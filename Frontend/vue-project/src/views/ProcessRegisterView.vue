<script setup>
    import ContentLimit from "../components/ContentLimit.vue"
    import http from "@/services/http"
    import auth from "@/services/auth"
    import { useRouter } from 'vue-router';
    import { ref } from "vue";

    const router = useRouter()

    async function checkToken() {
        const tokenIsValid = await auth();
        console.log(tokenIsValid)
        if (!tokenIsValid) {
            router.push('/Login');
        } 
    }

    const process = ref({
        "numeroProcesso": 0,
        "parteId": 0,
        "tema": "",
        "descricao": "",
        "valor": 0
    })

    async function registerProcess() {

        try {
            await http.post("/Processo", 
                JSON.stringify(process.value), 
                {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`,
                        'Content-Type': 'application/json'
                    }
                }
        );
        router.push({name: "process_search"})
        } catch (error) {
            console.error("Erro ao tentar registrar: ", error);
        }
    
    }

    checkToken();

</script>

<template>
    <ContentLimit>
        <div class="processRegister">
            <h2 class="title">Cadastrar Processo</h2>
            <form @submit.prevent="registerProcess" class="registerForm">
                <div class="inputBox">
                    <label for="processNumber" class="label">Numero Do Processo</label>
                    <input type="number" name="processNumber" id="processNumber" class="input" min="0" v-model="process.numeroProcesso">
                </div>
                <div class="inputBox">
                    <label for="processParteId" class="label">ID Do Cliente</label>
                    <input type="number" name="processParteId" id="processParteId" class="input" min="0" v-model="process.parteId">
                </div>
                <div class="inputBox">
                    <label for="processTheme" class="label">Tema</label>
                    <input type="text" name="processTheme" id="processTheme" class="input" v-model="process.tema">
                </div>
                <div class="inputBox">
                    <label for="processDescription" class="label">Descrição</label>
                    <textarea class="input" name="processDescription" id="processDescription" v-model="process.descricao"></textarea>
                </div>
                <div class="inputBox">
                    <label for="processValue" class="label">Valor</label>
                    <input type="number" name="processValue" id="processValue" class="input" min="0" v-model="process.valor">
                </div>
                <button class="button">Realizar Cadastro</button>
            </form>
        </div>
    </ContentLimit>
</template>

<style scoped>
    .processRegister {
        width: 100%;
        display: flex;
        flex-direction: column;
        gap: 15px;
    }
    .processInput {
        display: flex;
        align-items: center;
        gap: 15px;
    }
    .processAttach {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }
    .processAttached {
        display: flex;
        align-items: center;
        gap: 5px;
    }
    .processAttachments {
        display: flex;
        flex-direction: column;
        gap: 10px;
    }
    .iconFile, .iconClose {
        width: 20px;
    }
    .iconClose {
        fill: red;
        cursor: pointer;
    }
    .inputBox {
        width: 100%;
    }
    .input, .button {
        max-width: 500px;
    }
    .registerForm {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }
    
</style>
