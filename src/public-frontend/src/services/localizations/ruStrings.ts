import { Localization } from './enStrings';

export const ruStrings: Localization = {
  article: {
    favorite: 'Проголосовать',
    unfavorite: 'Убрать голос',
    delete: 'Удалить статью',
    edit: 'Редактировать статью',
    toggleFavorited: 'Переключить голос',
    readMore: 'Читать далее...',
    load: 'Загрузка статьи...',
  },
  articleEditor: {
    submitButtonText: 'Опубликовать Статью',
    title: 'Заголовок статьи',
    description: 'О чем будет статья?',
    body: 'Напишите вашу статью (markdown будет поддерживаться)',
    tag: "Введите название тега и нажмите 'Enter'",
    imageRemove: 'Убрать',
  },
  home: {
    banner: {
      logoTitle: 'Проводник',
      logoText: 'Место, где вы можете делиться своими знаниями.',
    },
    feed: {
      yourFeed: 'Ваша лента',
      globalFeed: 'Общая лента',
    },
    tags: {
      load: 'Загрузка тегов...',
      popularTags: 'Популярные теги',
    },
  },
  header: {
    logoTitle: 'проводник',
    login: 'Войти',
    register: 'Зарегистрироваться',
    newArticle: 'Новая статья',
    settings: 'Настройки',
    homePage: 'Главная',
  },
  footer: {
    attribution: 'Новостная платформа, разработаная {0}. Код и дезайн распространяются под лицензией MIT.',
    logo: 'проводник',
  },
  profile: {
    myArticles: 'Мои статьи',
    favoritedArticles: 'Понравившееся статьи',
    loading: 'Загрузка профиля...',
  },
  userInfo: {
    follow: 'Следить',
    unfollow: 'Перестать следить',
    editProfile: 'Редактировать настройки профиля',
  },
  pagination: {
    goToPage: 'Перейти на страницу под {0} номером',
  },
  settings: {
    updateSettings: 'Обновить настройки',
    image: 'Ссылка на аватар',
    username: 'Ваше пользовательское имя',
    bio: 'Ваша краткая биография',
    email: 'Электронная почта',
    password: 'Пароль',
    pageHeader: 'Ваши настройки',
    logout: 'Или нажмите что бы выйти.',
  },
  register: {
    username: 'Ваше пользовательское имя',
    email: 'Электронная почта',
    password: 'Пароль',
    loginOption: 'У вас уже есть аккаунт?',
    pageHeader: 'Зарегистрироваться',
    submit: 'Зарегистрироваться',
  },
  login: {
    email: 'Электронная почта',
    password: 'Пароль',
    registerOption: 'Вам нужен аккаунт?',
    pageHeader: 'Войти',
  },
  viewer: {
    loading: 'Загрузка статей...',
    notArticles: 'Нет статей... пока.',
  },
  comment: {
    login: 'Войдите',
    register: 'зарегистрируйтесь',
    info: '{0} или {1} чтобы оставить комментарий',
    post: 'Опубликовать комментарий',
    write: 'Напишите комментарий...',
    loading: 'Загрузка комментариев...',
    delete: 'Удалить комментарий {0}',
  },
};
