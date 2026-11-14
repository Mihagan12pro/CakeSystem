const API = 'http://localhost:5258/api/auth';
const app = document.querySelector('#app');
const toastRoot = document.querySelector('#toast-root');

const roleNames = {
  Client: 'Заказчик', Manager: 'Директор', PurchasingManager: 'Менеджер по закупкам',
  AccountManager: 'Менеджер по продажам', Baker: 'Мастер'
};

function notify(message, type = 'success') {
  const node = document.createElement('div');
  node.className = `toast ${type === 'error' ? 'error' : ''}`;
  node.textContent = message;
  toastRoot.append(node);
  setTimeout(() => node.remove(), 3600);
}

function decodeToken(token) {
  try {
    const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
    return JSON.parse(decodeURIComponent(escape(atob(payload))));
  } catch { return null; }
}

function currentUser() {
  const token = localStorage.getItem('cake_token');
  if (!token) return null;
  const payload = decodeToken(token);
  if (!payload || (payload.exp && payload.exp * 1000 <= Date.now())) {
    localStorage.removeItem('cake_token');
    return null;
  }
  return { token, login: payload.sub || 'Пользователь', role: payload.role || 'Client' };
}

async function request(path, body) {
  const response = await fetch(`${API}/${path}`, {
    method: 'POST', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(body)
  });
  const text = await response.text();
  if (!response.ok) {
    let message = text;
    try { const parsed = JSON.parse(text); message = parsed.title || parsed.detail || parsed.message || text; } catch {}
    if (response.status === 409) message = 'Пользователь с таким логином уже существует.';
    if (response.status === 400) message = 'Неверный логин или пароль.';
    throw new Error(message || `Ошибка запроса (${response.status})`);
  }
  return text;
}

function renderAuth(mode = 'signin') {
  const signup = mode === 'signup';
  app.innerHTML = `
    <main class="auth-shell">
      <section class="auth-visual">
        <div class="visual-grain"></div>
        <div class="brand"><span class="brand-mark">✳</span><span>CakeSystem</span></div>
        <div class="visual-copy">
          <div class="eyebrow">Производство · процессы · контроль</div>
          <h1>Каждый ингредиент — на своём месте.</h1>
          <p>Единое рабочее пространство для команды кондитерского производства. Управляйте процессами и держите важные данные под контролем.</p>
        </div>
        <div class="visual-footer"><span>CAKESYSTEM / WORKSPACE</span><span>01 — 2026</span></div>
      </section>
      <section class="auth-panel">
        <div class="auth-card">
          <div class="brand mobile-brand"><span class="brand-mark">✳</span><span>CakeSystem</span></div>
          <h2>${signup ? 'Создать аккаунт' : 'С возвращением'}</h2>
          <p class="auth-subtitle">${signup ? 'Заполните данные, чтобы зарегистрироваться в системе.' : 'Войдите в рабочее пространство CakeSystem.'}</p>
          <form id="auth-form">
            ${signup ? `<div class="field"><label for="fullname">Имя и фамилия</label><input id="fullname" name="fullName" autocomplete="name" placeholder="Например, Анна Иванова" /></div>` : ''}
            <div class="field"><label for="login">Логин</label><input id="login" name="login" autocomplete="username" required minlength="3" placeholder="Введите логин" /></div>
            <div class="field"><label for="password">Пароль</label><input id="password" name="password" type="password" autocomplete="${signup ? 'new-password' : 'current-password'}" required minlength="${signup ? 6 : 1}" placeholder="${signup ? 'Не менее 6 символов' : 'Введите пароль'}" /></div>
            <div class="form-error" id="form-error"></div>
            <button class="primary-btn" id="submit-btn" type="submit">${signup ? 'Зарегистрироваться' : 'Войти'} <span>→</span></button>
          </form>
          <p class="auth-switch">${signup ? 'Уже есть аккаунт?' : 'Впервые в системе?'} <button class="text-btn" id="switch-mode" type="button">${signup ? 'Войти' : 'Создать аккаунт'}</button></p>
        </div>
      </section>
    </main>`;

  document.querySelector('#switch-mode').addEventListener('click', () => renderAuth(signup ? 'signin' : 'signup'));
  document.querySelector('#auth-form').addEventListener('submit', async (event) => {
    event.preventDefault();
    const form = new FormData(event.currentTarget);
    const login = String(form.get('login') || '').trim();
    const password = String(form.get('password') || '');
    const button = document.querySelector('#submit-btn');
    const error = document.querySelector('#form-error');
    error.classList.remove('visible');
    button.disabled = true;
    button.textContent = signup ? 'Создаём аккаунт…' : 'Входим…';
    try {
      if (signup) {
        await request('sigup', { login, password, fullName: String(form.get('fullName') || '').trim() || null });
        notify('Аккаунт создан. Теперь войдите в систему.');
        renderAuth('signin');
      } else {
        const rawToken = await request('signin', { login, password });
        const token = rawToken.trim().replace(/^"|"$/g, '');
        if (!decodeToken(token)) throw new Error('Сервер вернул некорректный токен авторизации.');
        localStorage.setItem('cake_token', token);
        renderWorkspace();
      }
    } catch (e) {
      error.textContent = e.message || 'Не удалось выполнить запрос.';
      error.classList.add('visible');
      button.disabled = false;
      button.innerHTML = `${signup ? 'Зарегистрироваться' : 'Войти'} <span>→</span>`;
    }
  });
}

const navItems = [
  { id: 'overview', icon: '⌂', title: 'Обзор' },
  { id: 'components', icon: '▦', title: 'Компоненты' },
  { id: 'orders', icon: '▤', title: 'Заказы' },
  { id: 'team', icon: '♙', title: 'Сотрудники' }
];

function renderWorkspace(page = 'overview') {
  const user = currentUser();
  if (!user) return renderAuth();
  const initials = user.login.slice(0, 2).toUpperCase();
  const pageTitle = navItems.find(x => x.id === page)?.title || 'Обзор';
  app.innerHTML = `
    <div class="workspace">
      <aside class="sidebar">
        <div class="brand"><span class="brand-mark">✳</span><span>CakeSystem</span></div>
        <div class="nav-caption">Рабочее пространство</div>
        <nav class="nav-list" aria-label="Основная навигация">
          ${navItems.map(item => `<button class="nav-item ${item.id === page ? 'active' : ''}" data-page="${item.id}"><span class="nav-icon">${item.icon}</span>${item.title}</button>`).join('')}
        </nav>
        <div class="sidebar-bottom"><div class="profile-mini"><div class="avatar">${initials}</div><div class="profile-text"><strong>${escapeHtml(user.login)}</strong><span>${roleNames[user.role] || user.role}</span></div><button class="logout" id="logout" title="Выйти">↗</button></div></div>
      </aside>
      <main class="main-area">
        <header class="topbar"><div class="breadcrumb">CakeSystem <span> / </span> <strong>${pageTitle}</strong></div><div class="topbar-right"><span class="status-pill"><i class="status-dot"></i> Сессия активна</span></div></header>
        <div id="page-content"></div>
      </main>
    </div>`;
  document.querySelectorAll('[data-page]').forEach(button => button.addEventListener('click', () => renderWorkspace(button.dataset.page)));
  document.querySelector('#logout').addEventListener('click', () => { localStorage.removeItem('cake_token'); renderAuth(); });
  renderPage(page, user);
}

function renderPage(page, user) {
  const content = document.querySelector('#page-content');
  if (page === 'overview') {
    content.innerHTML = `
      <div class="page-heading"><div><h1>Добро пожаловать, ${escapeHtml(user.login)}</h1><p>Рабочая панель CakeSystem</p></div><span class="date-label">${new Intl.DateTimeFormat('ru-RU', { dateStyle: 'long' }).format(new Date())}</span></div>
      <section class="welcome-card"><div class="eyebrow">Панель управления</div><h2>Производство начинается с порядка.</h2><p>Здесь будут собраны ключевые данные и рабочие инструменты вашей роли. Сейчас доступна авторизация, а загрузка справочника компонентов выполняется фоновым сервисом.</p></section>
      <div class="section-title"><h3>Разделы системы</h3><span>Состояние реализации</span></div>
      <section class="module-grid">
        <article class="module-card"><div class="module-icon">▦</div><h4>Компоненты</h4><p>Справочник сырья и материалов, загружаемый из Excel в PostgreSQL.</p><span class="module-state">Импорт реализован</span></article>
        <article class="module-card"><div class="module-icon">▤</div><h4>Заказы</h4><p>Работа с заказами клиентов и этапами их выполнения.</p><span class="module-state pending">Backend ещё не реализован</span></article>
        <article class="module-card"><div class="module-icon">♙</div><h4>Пользователи</h4><p>Регистрация и вход. Новые пользователи получают роль заказчика.</p><span class="module-state">Авторизация реализована</span></article>
      </section>
      <div class="section-title"><h3>Текущие возможности</h3></div>
      <section class="info-panel"><h3>Подключение к API</h3><p>Форма входа и регистрации работает с существующими методами <strong>POST /api/auth/signin</strong> и <strong>POST /api/auth/sigup</strong>. JWT хранится в браузере до выхода или истечения срока действия. Список компонентов пока нельзя получить: в API отсутствует метод чтения справочника.</p></section>`;
  } else if (page === 'components') {
    content.innerHTML = `<div class="page-heading"><div><h1>Компоненты</h1><p>Справочник сырья, ингредиентов и материалов</p></div></div><section class="page-panel"><div class="empty-state"><div class="empty-icon">▦</div><h3>Справочник пока не подключён к интерфейсу</h3><p>Backend уже импортирует компоненты из файла <strong>Static/Components.xls</strong> в PostgreSQL. Но публичного API-метода для получения списка, поиска или изменения компонентов в текущей версии нет.</p><span class="endpoint-tag">GET /api/components — отсутствует</span></div></section>`;
  } else if (page === 'orders') {
    content.innerHTML = `<div class="page-heading"><div><h1>Заказы</h1><p>Управление заказами кондитерского производства</p></div></div><section class="page-panel"><div class="empty-state"><div class="empty-icon">▤</div><h3>Раздел ещё не реализован</h3><p>В текущем backend нет сущности заказа, репозитория или API-методов для создания и обработки заказов. Интерфейс не показывает тестовые данные, чтобы не смешивать макет с реальным состоянием системы.</p></div></section>`;
  } else {
    content.innerHTML = `<div class="page-heading"><div><h1>Сотрудники</h1><p>Пользователи и роли системы</p></div></div><section class="page-panel"><div class="empty-state"><div class="empty-icon">♙</div><h3>Управление пользователями пока недоступно</h3><p>Сейчас реализованы регистрация и вход. Регистрация назначает роль Client автоматически. API для просмотра списка пользователей и изменения ролей отсутствует.</p></div></section>`;
  }
}

function escapeHtml(value) {
  return String(value).replace(/[&<>"']/g, char => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[char]));
}

if (currentUser()) renderWorkspace(); else renderAuth();
