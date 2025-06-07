<script setup lang="ts">
import { onMounted, ref } from 'vue';
import service from '@/services';

const emit = defineEmits(['close'])
const transactions = ref<any>()

function close() {
    emit('close')
}

onMounted(async () => {
    transactions.value = await service.getTransactions()
});
</script>
<template>
    <v-btn color="grey" class="ml-3" @click="close">Close</v-btn>
    <v-list lines="two">
        <v-list-item v-for="transaction in transactions" :key="transaction.id" :subtitle="transaction.accountName"
            :title="transaction.amount">
        </v-list-item>
    </v-list>
</template>