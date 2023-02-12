export const enStrings = {
  article: {
    favorite: 'Favorite Article',
    unfavorite: 'Unfavorite Article',
    delete: 'Delete Article',
    edit: 'Edit Article',
    toggleFavorited: 'Toggle Favorite',
    readMore: 'Read more...',
    load: 'Loading article...',
  },
  articleEditor: {
    submitButtonText: 'Publish Article',
    title: 'Article Title',
    description: "What's this article about",
    body: 'Write your article (markdown will be supported)',
    tag: 'Enter tag and press enter',
    imageRemove: 'Remove image',
  },
  home: {
    banner: {
      logoTitle: 'Conduit',
      logoText: 'A place to share your knowledge.',
    },
    feed: {
      yourFeed: 'Your Feed',
      globalFeed: 'Global Feed',
    },
    tags: {
      load: 'Loading tags...',
      popularTags: 'Popular Tags',
    },
  },
  header: {
    logoTitle: 'conduit',
    login: 'Sign in',
    register: 'Sign up',
    newArticle: 'New Article',
    settings: 'Settings',
    homePage: 'Home',
  },
  footer: {
    attribution: 'Blogging ecosystem developed by {0}. Code & design licensed under MIT.',
    logo: 'conduit',
  },
  profile: {
    myArticles: 'My Articles',
    favoritedArticles: 'Favorited Articles',
    loading: 'Loading profile...',
  },
  userInfo: {
    follow: 'Follow',
    unfollow: 'Unfollow',
    editProfile: 'Edit Profile Settings',
  },
  pagination: {
    goToPage: 'Go to page number {0}',
  },
  settings: {
    updateSettings: 'Update Settings',
    image: 'URL of profile picture',
    username: 'Your Name',
    bio: 'Short bio about you',
    email: 'Email',
    password: 'Password',
    pageHeader: 'Your Settings',
    logout: 'Or click here to logout.',
  },
  register: {
    username: 'Your Name',
    email: 'Email',
    password: 'Password',
    loginOption: 'Have an account?',
    pageHeader: 'Sign up',
    submit: 'Sign up',
  },
  login: {
    email: 'Email',
    password: 'Password',
    registerOption: 'Need an account?',
    pageHeader: 'Sign in',
  },
  viewer: {
    loading: 'Loading articles...',
    notArticles: 'No articles are here... yet.',
  },
  comment: {
    login: 'Sign in',
    register: 'sign up',
    info: '{0} or {1} to add comments on this article.',
    post: 'Post Comment',
    write: 'Write a comment...',
    loading: 'Loading comments...',
    delete: 'Delete comment {0}',
  },
};

export type Localization = typeof enStrings;
