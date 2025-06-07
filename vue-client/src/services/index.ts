const baseUrl = "http://localhost:5203";
export interface Account {
  id: number;
  name: string;
  balance: number;
}

export default {
  async getAccounts(): Promise<Account[]> {
    const response = await fetch(`${baseUrl}/accounts`);
    const data = await response.json();
    return data;
  },
  async getTransactions() {
    const response = await fetch(`${baseUrl}/transactions`);
    const data = await response.json();
    return data;
  },
  async postTransaction(accountId: number, amount: number) {
    const response = await fetch(`${baseUrl}/transactions`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({ accountId, amount }),
    });
    return response;
  },
  async transferFunds(
    fromAccountId: number,
    toAccountId: number,
    amount: number
  ) {
    const response = await fetch(`${baseUrl}/transfers`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify({
        fromAccountId,
        toAccountId,
        amount,
      }),
    });

    return response;
  },
};
