<!-- Delimitar o tamanho maximo do conteudo da pagina em 1440px de largura -->

<template>
    <div class="body">
        <div class="box">
            <slot></slot>
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
        padding: 15px;
        height: 100%;
        background-color: var(--color-primary);
    }
</style>