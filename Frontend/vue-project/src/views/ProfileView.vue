<script setup>
    import ContentLimit from "../components/ContentLimit.vue"
    import auth from "@/services/auth"
    import http from "@/services/http"
    import { useRouter } from 'vue-router';
    import { ref } from 'vue';

    const router = useRouter()
    const user = ref();
    const id = ref();
    const funcao = ref();
    const password = ref({
        oldPassword: "",
        newPassword: ""
    })
    const feedback = ref(null);

    async function checkToken() {
        const tokenIsValid = await auth();
        if (!tokenIsValid) {
            router.push('/Login');
        } else {
        pessoaInfo()
        }
    }

    async function pessoaInfo() {
        const pessoa = await http.get("/Pessoa/Info", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        user.value = pessoa.data.nome
        id.value = pessoa.data.id
        funcao.value = pessoa.data.oab == null ? "Cliente" : "Advogado"
    }

    checkToken();

    async function changePassword() {
        try {
        await http.post("/Login/ChangePassword", 
            JSON.stringify({
                currentPassword: password.value.oldPassword,
                newPassword: password.value.newPassword
            }), 
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`,
                    'Content-Type': 'application/json'
                }
            }
        );
        feedback.value = true
    } catch (error) {
        feedback.value = false
        console.error("Erro ao alterar senha:", error);
    }
    }

</script>

<template>
    <ContentLimit>
        <div class="profile">
            <h2 class="title">Perfil</h2>
            <div class="profileUser">
                <img src="../assets/user.jpg" alt="Sua Foto De Perfil" class="profileImage">
                <div class="profileInfo">
                    <span class="profileName text">Nome : {{user}} ({{ id }})</span>
                    <span class="profileFuction text">Função : {{funcao}}</span>
                </div>
            </div>
            <div class="profileEdit">
                <h2 class="title">Alterar Senha</h2>
                <form @submit.prevent="changePassword" class="form">
                    <div class="inputBox">
                        <label for="oldPassword" class="label">Senha antiga</label>
                        <input type="text" name="oldPassword" id="oldPassword" class="input" v-model="password.oldPassword">
                    </div>
                    <div class="inputBox">
                        <label for="newPassword" class="label">Senha nova</label>
                        <input type="text" name="newPassword" id="newPassword" class="input" v-model="password.newPassword">
                    </div>
                    <button class="button profileSaveButton">Alterar</button>
                    
                    <span class="textErro" v-if="feedback == true">Alterado com sucesso!</span>
                    <span class="textSucess" v-else-if="feedback == false">Senha invalida!</span>
                    
                </form>
            </div>
        </div>
    </ContentLimit>
</template>

<style scoped>

    .profileUser {
        display: flex;
        margin: 15px 0;
    }
    .profileInfo {
        display: flex;
        flex-direction: column;
        justify-content: center;
        gap: 15px;
        margin-left: 15px;
    }
    .profileImage {
        width: 192px;
        clip-path: circle(50% at 50% 50%);
    }
    .profileButton {
        padding: 5px;
    }
    .imageChange {
        display: flex;
        align-items: center;
        gap: 10px;
    }
    .imageInput {
        display: none;
    }
    .profileEdit {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }
    .profileSaveButton {
        width: 300px;
    }
    @media (max-width: 500px) {
        .profile {
            width: 100%;
        }
        .profileUser {
            flex-direction: column;
        }
        .profileImage {
            align-self: center
        }
        .profileInfo {
            margin-left: 0;
        }
    }
</style>