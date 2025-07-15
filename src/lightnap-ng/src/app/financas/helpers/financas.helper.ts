
import { Financas } from "../models/response/financas";

export class FinancasHelper {
  static rehydrate(financas: Financas) {
    if (!financas) return;

    if (financas.data) {
      financas.data = new Date(financas.data);
    }
  }
}
