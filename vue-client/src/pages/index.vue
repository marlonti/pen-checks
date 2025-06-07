<script lang="ts" setup>
import service, { type Account } from '@/services'
import { onMounted, ref } from 'vue'

const accounts = ref<Account[]>()
const showTransferOptions = ref(false)
const showHistory = ref(false)

async function fetchAccounts() {
  accounts.value = await service.getAccounts()
}
async function closeTransferOptions(refresh: boolean) {
  if (refresh)
    fetchAccounts()

  showTransferOptions.value = false
}

onMounted(async () => {
  await fetchAccounts()
})
</script>

<template>
  <v-container>
    <h1>Fells Wargo Bank</h1>
    <v-row>
      <v-col v-for="account in accounts" cols="6">
        <BankAccount :model="account" @updated="fetchAccounts" />
      </v-col>
    </v-row>
    <v-row>
      <v-col>
        <v-btn @click="showTransferOptions = !showTransferOptions">Transfer Funds</v-btn>
        <v-btn @click="showHistory = !showHistory" class="ml-3">Transaction History</v-btn>
      </v-col>
    </v-row>
    <v-row v-if="showTransferOptions && accounts">
      <v-col>
        <TransferFunds :accounts="accounts" @updated="closeTransferOptions(true)" @close="closeTransferOptions" />
      </v-col>
    </v-row>
    <v-row v-if="showHistory">
      <v-col>
        <TransactionHistory @close="showHistory = false" />
      </v-col>
    </v-row>
  </v-container>
</template>
