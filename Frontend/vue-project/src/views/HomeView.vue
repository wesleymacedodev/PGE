<!-- Pagina Inicial -->

<script setup>
  import ContentLimit from "../components/ContentLimit.vue"
  import auth from "@/services/auth"
  import http from "@/services/http"
  import { useRouter } from 'vue-router';
  import { ref } from 'vue';

  const router = useRouter()
  const user = ref();

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
  }

  checkToken();

</script>

<template>
  <ContentLimit >
    <div class="home">
      <div class="content">
        <h2 class="title">Olá {{user}},</h2>
        <p class="text">Este sistema foi desenvolvido para facilitar o gerenciamento de processos, permitindo que você tenha controle total sobre suas atividades relacionadas a processos, clientes e documentos.</p>
        <p class="text">Aqui estão algumas das principais funcionalidades que você pode explorar:</p>
        <ul class="list">
          <li class="element">Visualizar Processos Recentes: Veja os processos mais recentes relacionados a você.</li>
          <li class="element">Criar Novos Processos: Adicione novos processos com facilidade.</li>
          <li class="element">Detalhes dos Processos: Visualize detalhes específicos de cada processo e os documentos associados a eles.</li>
          <li class="element">Atualizar Perfil: Mantenha suas informações atualizadas para uma experiência personalizada.</li>
        </ul>
        <p class="text">Estamos empolgados em tê-lo(a) aqui e esperamos que este sistema torne sua gestão de processos mais eficiente e organizada. Se precisar de ajuda ou tiver alguma dúvida, nossa equipe está à disposição para ajudá-lo.</p>
        <span class="text">Aproveite sua experiência!</span>
      </div>
      <img src="../assets/judicial.jpg" alt="Imagem Ilustrativa De Algo Judicial" class="image">
    </div>
  </ContentLimit>
</template>

<style scoped>
  .home {
    display: flex;
    justify-content: space-between;
    align-items: center;
    width: 100%;
  }

  .content {
    display: flex;
    min-width: 300px;
    max-width: 600px;
    flex-direction: column;
    gap: 20px;
  }
  .list {
    margin-left: 15px;
  }
  .image {
    min-width: 256px;
    max-width: 320px;
    object-fit: contain;
  }
  @media (max-width: 1080px) {
    .home {
      flex-direction: column;
    }
    .image {
      order: 1;
    }
    .content {
      order: 2;
    }
    .title {
      font-size: var(--mfont-size-1);
      font-weight: bold;
    }
    .text {
      font-size: var(--mfont-size-2);
    }
    .list .element {
      font-size: var(--mfont-size-3);
    }
    .span {
      font-size: var(--mfont-size-4);
      font-weight: bold;
    }

  }
</style>