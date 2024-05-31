<script setup>
    import ContentLimit from "../components/ContentLimit.vue"
    import http from '@/services/http'
    import auth from '@/services/auth'
    import { ref } from 'vue';
    import { useRoute, useRouter } from "vue-router";

    const router = useRouter()
    const route = useRoute()
    const processId = route.params.id
    const funcao = ref(false)
    const files = ref([]);
    const process = ref({
        "id": 0,
        "numeroProcesso": 0,
        "parteId": 0,
        "responsavelId": 0,
        "tema": "",
        "descricao": "",
        "valor": 0
    })
    const userId = ref();
    const fileActions = ref();
    

    async function checkToken() {
        const tokenIsValid = await auth();
        if (!tokenIsValid) {
            router.push('/Login');
        } else {
            loadProcess()
            loadAttachment()
        }
    }

    async function loadProcess () {
        try {
            const response = await http.get(`/Processo/${processId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }
            })
            process.value = response.data
            pessoaInfo()
        } catch (error) {
            console.log("Não foi possivel visualizar o processo!"+error);
        }
    }

    async function pessoaInfo() {
        const pessoa = await http.get("/Pessoa/Info", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        funcao.value = pessoa.data.oab == null ? false : true
        userId.value = pessoa.data.id
        fileActions.value = process.value.parteId == userId.value || process.value.responsavelId == userId.value ? true : false
    }

    async function loadAttachment() {
        const documents = await http.get(`/Documento/Processo/${processId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        files.value = documents.data
    }

        async function downloadDocument(id) {
        try {
            const response = await http.get(`/Documento/Download/${id}`, {
                responseType: 'blob',
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }
            });

            const contentDisposition = response.headers['content-disposition'];

            const fileName = getFileNameFromContentDisposition(contentDisposition);

            const url = window.URL.createObjectURL(new Blob([response.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', fileName); 
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
        } catch (error) {
            console.error("Erro ao tentar baixar o documento: ", error);
        }
    }

    function getFileNameFromContentDisposition(contentDisposition) {
        let fileName = "documento";
        const fileNameMatch = contentDisposition.match(/filename\*?=(?:UTF-8''|['"]?)([^;\r\n"']*)/);
        if (fileNameMatch !== null && fileNameMatch.length > 1) {
            fileName = decodeURIComponent(fileNameMatch[1].replace(/['"]/g, ''));
        }
        return fileName;
    }

    async function deleteProcess() {
        try {
            await http.delete(`/Processo/${processId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
            router.push({name:"process_search"})
        } catch (error) {
            console.log("Não foi possivel deletar : " + error)
        }
    }

    function goToProcessEdit() {
        router.push({name: 'process_edit'})
    }


    checkToken()

</script>

<template>
    <ContentLimit>
        <div class="processView">

            <h2 class="title">Destalhes Do Processo - {{ process.id }}</h2>
            <div class="processBox">
                <div class="processInfo">
                    <span class="text">Tema : {{ process.tema }} </span>
                    <span class="text">Descrição : {{ process.descricao }} </span>
                    <span class="text">Cliente : {{ process.parteId }}</span>
                    <span class="text">Responsavel : {{ process.responsavelId }} </span>
                    <span class="text">Valor : {{ process.valor }}</span>
                    <div class="attachmentBox">
                        <span class="text">Anexos</span>
                        <div v-for="x in files" :key="x.id" class="attachmentItem">
                            <span class="text">{{ x.nome }}</span>
                            <span @click="downloadDocument(x.id)" class="text link">( baixar )</span>
                        </div>
                    </div>
                </div>
                <div class="processActions" v-show="fileActions">
                    <button class="button " @click="goToProcessEdit">Editar</button>
                    <button class="button" v-show="funcao" @click="deleteProcess">Deletar</button>
                </div>
            </div>
        </div>
    </ContentLimit>
</template>

<style scoped>
    .processView {
        width: 100%;
        display: flex;
        flex-direction: column;
        gap: 20px;
    }
    .processInfo {
        display: flex;
        flex-direction: column;
        padding: 15px;
        background-color: var(--input-color);
    }
    .processActions {
        display: flex;
    }
    .button {
        width: 100%;
    }

    .edit {
        background-color: var(--edit-color);
        color: var(--edit-color);
    }
    .delete {
        background-color: var(--button-color);
        color: var(--delete-color);;
    }   

    .attachmentBox {
        background-color: var(--input-color-hover);
        padding: 15px;
    }

    .attachmentItem {
        display: flex;
        gap: 5px;
        align-items: center;
    }

    .link {
        color: var(--link-color);
        text-decoration: underline;
        cursor: pointer;
    }


</style>