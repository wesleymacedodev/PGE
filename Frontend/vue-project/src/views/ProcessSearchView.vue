<script setup>
import ContentLimit from "../components/ContentLimit.vue"
import IconSearchBar from "../components/icons/IconSearchBar.vue"
import auth from "@/services/auth"
import http from "@/services/http"
import { useRouter } from 'vue-router';
import { ref, computed } from 'vue';

const router = useRouter()
const processList = ref([])
const searchField = ref("");
const funcao = ref(false);
const responsavelId = ref(null);
const showAllProcesses = ref(false);

async function checkToken() {
    const tokenIsValid = await auth();
    if (!tokenIsValid) {
        router.push('/Login');
    } else {
      pessoaInfo()
      loadProcess()
    }
}

async function pessoaInfo() {
        const pessoa = await http.get("/Pessoa/Info", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
        funcao.value = pessoa.data.oab == null ? false : true
        responsavelId.value = pessoa.data.id
      }

async function loadProcess() {
    const processo = await http.get("/Processo/List", {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem("token")}`
                }})
    processList.value = processo.data
}

function goToProcessView(processId) {
    router.push({ name: 'process', params: { id: processId } });
}

function goToProcessRegister() {
    router.push({ name: 'process_register' })
}

checkToken();

const filterProcess = computed(() => {
  if (processList.value) {
    return processList.value.filter(processValue => {
      const matchesSearchField = searchField.value 
      ? String(processValue.numeroProcesso).includes(searchField.value)
      : true;
      if (funcao.value) {
        const matchesResponsavel = showAllProcesses.value || processValue.responsavelId === responsavelId.value;
        return matchesResponsavel && matchesSearchField;
      }
      return matchesSearchField
    });
  }
  return processList.value;
});

</script>

<template>
    <ContentLimit>
        <div class="processSearch">
            <div class="header">
                <h2 class="title">Seus Processos</h2>
                <button class="button" @click="goToProcessRegister" v-show="funcao" >Cadastrar</button>
            </div>
            <div class="search">
                <input type="text" name="" id="" class="searchInput input" placeholder="Pesquisar Numero De Processo" v-model="searchField">
                <button class="inputButton"><IconSearchBar class="searchIcon" /></button>
            </div>
            <div class="option" v-show="funcao">
              <input type="checkbox" name="showAllProcess" id="showAllProcess" class="showAllProcess"  v-model="showAllProcesses">
              <label for="showAllProcess" class="text">Mostrar processos de todos advogados</label>
            </div>
            <div class="processList">
                <div class="processBox" v-for="process in filterProcess" :key="process.id" @click="goToProcessView(process.id)">
                    <span class="text">ID : {{ process.id }}</span>
                    <span class="text">Numero Do Processo : {{ process.numeroProcesso }}</span>
                    <span class="text">Cliente : {{ process.parteId }}</span>
                    <span class="text">Responsavel : {{ process.responsavelId }} </span>
                    <p class="text">Tema : {{ process.tema }}</p>
                </div>
            </div>
        </div>
    </ContentLimit>
</template>

<style scoped>
  .processSearch {
    display: flex; 
    flex-direction: column;
    width: 100%;
    gap: 20px;
  }
  .header {
    display: flex;
    align-items: center;
    gap: 15px;
  }
  .search {
    display: flex;
    }
  .searchIcon {
    width: 24px;
  }
  .searchInput {
    width: 100%;
    max-width: 500px;
  }
  .processList {
    display: flex;
    flex-direction: column;
    gap: 15px;
  }
  .processBox {
    max-width: 600px;
    display: flex;
    flex-direction: column;
    background-color: var(--input-color);
    padding: 15px;
    cursor: pointer;
    transition: transform .5s;
  }

  .processBox:hover {
    transform: scale(1.03);
    background-color: var(--input-color-hover);
  }
  .option {
    display: flex;
    align-items: center;
    gap: 15px;
  }

</style>