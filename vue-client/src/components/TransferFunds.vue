<script lang="ts" setup>
import type { Account } from '@/services';
import service from '@/services';
import { computed, ref, toRefs } from 'vue';

const emit = defineEmits(['updated', 'close'])
const props = defineProps<{ accounts: Account[] }>()
const { accounts } = toRefs(props)

const errorMessage = ref<string>()
const accountFrom = ref<number>()
const accountTo = ref<number>()
const amount = ref<number>()

async function submit() {
  if (!accountFrom.value) {
    errorMessage.value = 'Account Source is required.'
    return
  }
  if (!accountTo.value) {
    errorMessage.value = 'Account Destination is required.'
    return
  }
  if (accountFrom.value === accountTo.value) {
    errorMessage.value = 'Cannot send money to same account.'
    return
  }
  if (!amount.value) {
    errorMessage.value = 'Amount is required.'
    return
  }

  const response = await service.transferFunds(accountFrom.value, accountTo.value, amount.value)
  if (!response.ok) {
    console.log(response)
    const data = await response.json();
    if (data)
      errorMessage.value = data

    return
  }
  errorMessage.value = undefined
  emit('updated')
}

function close() {
  emit('close')
}
</script>

<template>
  <v-card>
    <v-card-title>Transfer</v-card-title>
    <v-form @submit.prevent="submit">
      <v-alert v-if="!!errorMessage" type="error" :text="errorMessage"></v-alert>
      <v-select v-model="accountFrom" :items="accounts" item-title="name" item-value="id"
        label="From Account"></v-select>
      <v-select v-model="accountTo" :items="accounts" item-title="name" item-value="id" label="To Account"></v-select>

      <v-text-field v-model="amount" label="Amount to Transfer" type="number"></v-text-field>
      <v-btn color="success" type="submit">Save</v-btn>
      <v-btn color="grey" class="ml-3" @click="close">Cancel</v-btn>
    </v-form>
  </v-card>
</template>