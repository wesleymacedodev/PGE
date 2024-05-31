// Rotas do site com todas as views

import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LoginView from '../views/LoginView.vue'
import ProcessView from '../views/ProcessView.vue'
import ProcessSearchView from '../views/ProcessSearchView.vue'
import ProcessRegisterView from '../views/ProcessRegisterView.vue'
import ProcessEditView from '../views/ProcessEditView.vue'
import ProfileView from '../views/ProfileView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
      meta: { auth: true}
    },
    {
      path: '/profile',
      name: 'profile',
      component: ProfileView,
      meta: { auth: true}
    },
    {
      path: '/login',
      name: 'login',
      component: LoginView,
      meta: { hideNavbar: true }
    },
    {
      path: '/process/:id',
      name: 'process',
      component: ProcessView,
      meta: { auth: true},
      props: true
    },
    {
      path: '/process/search',
      name: 'process_search',
      component: ProcessSearchView,
      meta: { auth: true}
    },
    {
      path: '/process/register',
      name: 'process_register',
      component: ProcessRegisterView,
      meta: { auth: true}
    },
    {
      path: '/process/edit/:id',
      name: 'process_edit',
      component: ProcessEditView,
      meta: { auth: true},
      props: true
    },
    { path: '/:pathMatch(.*)*', redirect: '/' }
  ]
})

// Protegendo rotas
router.beforeEach((to, from, next) => {
  const loggedIn = localStorage.getItem('token');
  if (to.matched.some(record => record.meta.auth) && !loggedIn) {
    next('/login');
  } else {
    next();
  }
});

export default router
