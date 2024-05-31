<script setup>
import { RouterView, useRoute } from 'vue-router'
import { computed } from 'vue';
import NavBar from './components/NavBar.vue'

const menuItems = [
  {text:"Inicio", link:"/"},
  {text:"Processos", link:"/process/search"},
  {text:"Perfil", link:"/profile"},

]

// Verificação para não exibir navbar caso a rota seja /login
const route = useRoute();
const showNavBar = computed(() => !route.meta.hideNavbar);

</script>

<template>

  <NavBar v-if="showNavBar" :menuItems="menuItems"></NavBar>

  <RouterView />
</template>

