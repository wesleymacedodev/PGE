<!-- Componente da navbar com link das rotas -->

<script setup>
import IconLogo from './icons/navbar/IconLogo.vue'
import IconMenu from './icons/navbar/IconMenu.vue'
import { RouterLink, useRoute } from 'vue-router'
import { defineProps, ref } from 'vue'

const logout = () => {
  localStorage.removeItem("token");
  useRoute().route.push("/login")
}

// Rotas passadas pelo App.vue
defineProps({
  menuItems: {
    type: Array,
    require: true
  }
})

// Interação com menu hamburguer
const isOpenMenu = ref(false)
const toggleHandler = () => {
  isOpenMenu.value = !isOpenMenu.value
}



</script>

<template>
  <header>
    <div class="navbar">
      <RouterLink to="/">
        <IconLogo class="logo" />
      </RouterLink>
      <IconMenu @click="toggleHandler" class="mobile-menu" />
      <ul class="items">
        <li v-for="item in menuItems" :key="item.text" class="item">
          <RouterLink :to="item.link">{{ item.text }}</RouterLink>
        </li>
        <li class="item" @click="logout">
          <a href="">Sair</a>
        </li>
      </ul>
    </div>

    <!-- Menu Mobile -->

    <ul class="mobile-items" :class="isOpenMenu == true ? 'showMobile' : 'hideMobile'">
      <li v-for="item in menuItems" :key="item.text" class="item">
        <RouterLink :to="item.link">{{ item.text }}</RouterLink>
      </li>
      <li class="item" @click="logout">
          <a href="">Sair</a>
        </li>
    </ul>

    <!-- Detalhes da logo -->
    <div class="navbar-details">
      <div class="navbar-detail-1"></div>
      <div class="navbar-detail-2"></div>
      <div class="navbar-detail-3"></div>
    </div>
  </header>
</template>

<style scoped>
header {
  background-color: var(--navbar-color-primary);
}
.navbar {
  display: flex;
  align-items: center;
  justify-content: space-between;
  width: 100%;
  max-width: 1440px;
  height: 60px;
  margin: 0 auto;
}
.navbar .logo {
  height: 40px;
  width: 178px;
  margin-left: 15px;
}

.navbar .items,
.mobile-items {
  display: flex;
  list-style: none;
  gap: 15px;
  margin-right: 15px;
}

.navbar .item a,
.mobile-items .item a {
  font-size: var(--font-size-2);
  color: var(--font-color-1);
  text-decoration: none;
}

.mobile-menu {
  display: none;
  margin-right: 15px;
}

.mobile-items {
  display: none;
  background-color: var(--navbar-color-primary);
  margin: 0;
  transition: height 1s;
}

.hideMobile {
  height: 0;
}

.showMobile {
  height: 300px;
}

.navbar-details {
  display: flex;
}
.navbar-details div {
  height: 5px;
  width: 100%;
}
.navbar-detail-1 {
  background: var(--navbar-detail-color-1);
}
.navbar-detail-2 {
  background: var(--navbar-detail-color-2);
}
.navbar-detail-3 {
  background: var(--navbar-detail-color-3);
}

@media (max-width: 500px) {
  .navbar .items {
    display: none;
  }
  .mobile-menu {
    display: block;
  }
  .mobile-items {
    display: flex;
    flex-direction: column;
    align-items: center;
    justify-content: center;
    z-index: -1;
    overflow: hidden;
  }
}
</style>
