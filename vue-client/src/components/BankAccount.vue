<script setup lang="ts">
import type { Account } from '@/services';
import service from '@/services';
import { computed, ref, toRefs } from 'vue';

const emit = defineEmits(['updated'])
const props = defineProps<{ model: Account }>()
const { model } = toRefs(props)

const action = ref<number>()
const inputAmount = ref<number>()
const errorMessage = ref<string>()
const showInput = computed(() => !!action.value)
const inputLabel = computed(() => action.value === 1 ? 'Deposit Amount' : 'Withdrawal Amount')

async function saveInput() {
    //Add validation

    let amount = 0;
    if (action.value === 1)
        amount = inputAmount.value!
    else if (action.value === 2)
        amount = inputAmount.value! * -1

    const response = await service.postTransaction(model.value.id, amount)
    if (!response.ok) {
        console.log(response)
        const data = await response.json();
        if (data)
            errorMessage.value = data

        return
    }

    emit('updated')
    clearInput()
}
function clearInput() {
    inputAmount.value = undefined
    action.value = undefined
    errorMessage.value = undefined
}
</script>
<template>
    <v-card>
        <template v-slot:title>
            {{ model.name }}
        </template>
        <template v-slot:subtitle>
            ${{ model.balance }}
        </template>
        <template v-slot:text>
            <v-btn color="indigo" @click="action = 1">Deposit</v-btn>
            <v-btn color="indigo" class="ml-3" @click="action = 2">Withdraw</v-btn>
            <v-row v-if="showInput" class="mt-5">
                <v-col>
                    <v-alert v-if="!!errorMessage" type="error" :text="errorMessage"></v-alert>
                    <v-text-field v-model="inputAmount" :label="inputLabel" type="number"></v-text-field>
                    <v-btn color="success" @click="saveInput">Save</v-btn>
                    <v-btn color="grey" class="ml-3" @click="clearInput">Cancel</v-btn>
                </v-col>
            </v-row>
        </template>
    </v-card>
</template>