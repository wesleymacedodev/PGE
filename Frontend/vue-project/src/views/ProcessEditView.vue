<script setup>
    import ContentLimit from "../components/ContentLimit.vue"
    import IconFile from "../components/icons/IconFile.vue"
    import IconClose from "../components/icons/IconClose.vue"
    import http from "@/services/http"
    import auth from "@/services/auth"
    import { useRouter, useRoute } from 'vue-router';
    import { ref } from "vue";

    const router = useRouter()
    const route = useRoute()
    const funcao = ref(false)
    const userId = ref()
    const processId = route.params.id
    const processAttachments = ref([])

    async function checkToken() {
        const tokenIsValid = await auth();
        if (!tokenIsValid) {
            router.push('/Login');
        } else {
            loadProcess()
            pessoaInfo()
            loadAttachment()
        }
    }

    const process = ref({
        "numeroProcesso": 0,
        "parteId": 0,
        "tema": "",
        "descricao": "",
        "valor": 0
    })

    const distribute = ref({
        "processoId": 0,
        "responsavelAntigoId": 0,
        "responsavelNovoId": 0,
    })

    const files = ref([]);

    function handleFileChange(event) {
        files.value = Array.from(event.target.files);
    }

    function removeFile(index) {
        files.value.splice(index, 1);
    }

    async function registerDocument() {
        try {
            if(files.value != null) {
                const formData = new FormData();
                formData.append("file", files.value[0]);
                formData.append("processoId", processId);

                await http.post("/Documento", 
                    formData, 
                    {
                        headers: {
                            Authorization: `Bearer ${localStorage.getItem("token")}`,
                            'Content-Type': 'multipart/form-data'
                        }
                    }
                );
            }
        } catch (error) {
            console.error("Erro ao tentar adicionar anexo : ", error);
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
        } catch (error) {
            console.log("Não foi possivel visualizar o processo : " + error);
        }
    }

    async function pessoaInfo() {
        const pessoa = await http.get("/Pessoa/Info", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        funcao.value = pessoa.data.oab == null ? false : true
        userId.value = pessoa.data.id
    }

    async function editProcess () {
        try {
            await registerDocument()
            if (funcao.value) {       
                await http.put(`/Processo`, 
                JSON.stringify(process.value), 
                {    
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem("token")}`
                    }
                })
            }
            router.go(0)
        } catch (error) {
            console.log("Não foi possivel editar o processo : " + error);
        }
    }

    async function distributeProcess () {
        try {
            distribute.value.processoId = processId
            distribute.value.responsavelAntigoId = userId.value
            await http.post("/Distribuir", 
                JSON.stringify(distribute.value),
            {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }
            })
            router.push("process_search")
        } catch (error) {
            console.log("Não foi possivel distribuir : " + error);
        }
    }

    async function loadAttachment() {
        const documents = await http.get(`/Documento/Processo/${processId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        processAttachments.value = documents.data
    }

    async function deleteAttachment(attachId, index) {
        console.log(attachId)
        try {
            await http.delete(`/Documento/${attachId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
                processAttachments.value.splice(index, 1)
        } catch (error) {
            console.log("Ocorreu um erro ao excluir o anexo :" + error)
        }
    }

    checkToken();


</script>

<template>
    <ContentLimit>
        <div class="processRegister">
            <div class="header">
                <h2 class="title">Editar Processos - {{ process.id }}</h2>
            </div>
            <form @submit.prevent="editProcess" class="editForm">
                <div v-show="funcao" class="editForm">
                    <div>
                        <button class="button" @click="distributeProcess" v-show="funcao" >Distribuir</button>
                        <input type="text" class="input distributeInput" v-model="distribute.responsavelNovoId">
                    </div>

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
                </div>
                <div class="processAttachBox">
                    <div class="processInput">
                        <span class="text">Anexos</span>
                        <input type="file" @change="handleFileChange" style="display: none;" ref="fileInput">
                        <button type="button" class="button" @click="$refs.fileInput.click()">Adicionar</button>
                    </div>
                    <div class="processAttachments">
                        <div class="processAttach">
                            <span class="text">Submeter</span>
                            <div v-for="(file, index) in files" :key="index" class="processAttached">
                                <IconFile class="iconFile"/>
                                <span class="text">{{ file.name }}</span>
                                <IconClose class="iconClose" @click="removeFile(index)"/>
                            </div>
                        </div>
                        <div class="processAttach">
                            <span class="text">Submetidos</span>
                            <div v-for="(document, index) in processAttachments" :key="document.id" class="attachBox">
                                <IconFile class="iconFile"/>
                                <span class="text">{{ document.nome }}</span>
                                <IconClose class="iconClose" @click="deleteAttachment(document.id, index)"/>
                            </div>
                        </div>
                    </div>
                </div>
                <button class="button">Realizar Alteração</button>
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
    .processAttachBox {
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
    .distributeInput {
        font-size: var(--font-size-3);
    }
    .editForm {
        display: flex;
        flex-direction: column;
        gap: 15px;
    }
    .processAttach {
        display: flex;
        flex-direction: column;
        gap: 15px;
        padding: 15px;
        background-color: var(--input-color-hover);
        max-width: 500px
    }
    .attachBox {
        display: flex;
        align-items: center;
        gap: 10px;
    }

</style>
