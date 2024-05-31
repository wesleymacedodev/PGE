<!-- Pagina De Login -->

<script setup>

import http from "@/services/http"
import { reactive, ref } from "vue"
import { useRouter } from 'vue-router';

const router = useRouter()

const user = reactive({
    nome: "",
    password: ""
})

const feedback = ref(false);

async function login() {
    try {
        const {data} = await http.post("/Login", user)
        localStorage.setItem('token', data.token);
        router.push('/');
    } catch (error) {
        feedback.value = true
        console.log(error?.response?.data)
    }
}

</script>

<template>
    <div class="body">
        <div class="box">
            <img src="../assets/local.jpg" alt="Imagem Do PGE" class="content">
            <div class="login">
                <img src="../assets/logo_github.png" alt="Logo Da Tela De Login" class="logo">
                <form @submit.prevent="login" class="form">
                    <div class="inputBox">
                        <label for="user" class="label">Usuário</label>
                        <input type="text" name="user" id="user" v-model="user.nome" class="input" placeholder="Adicione o seu usuário!">
                    </div>
                    <div class="inputBox">
                        <label for="password" class="label">Senha</label>
                        <input type="text" name="password" id="password" v-model="user.password" class="input" placeholder="Adicione a sua senha!">
                    </div>
                    <button class="button">Conectar</button>
                    <span class="textErro" v-show="feedback">Não foi possivel logar!</span>
                </form>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .body {
        display: flex;
        justify-content: center;
    }
    .box {
        display: flex;
        width: 100%;
        max-width: 1440px;
        height: 100vh;
        background-color: var(--color-primary);
    }
    .content {
        width: 50%;
        object-fit: cover;
        object-position: 35%;
    }
    .login {
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
        width: 50%;
    }
    .login .logo {
        width: 128px;
        clip-path: circle(50% at 50% 50%);
        margin-bottom: 40px;
    }

    @media (max-width: 768px) {
        .content { 
            display: none;
        }
        .login {
            width: 100%;
        }
    }


</style>