
import { inject, Injectable } from "@angular/core";
import { CreateFinancasRequest } from "../models/request/create-financas-request";
import { SearchFinancasRequest } from "../models/request/search-financas-request";
import { UpdateFinancasRequest } from "../models/request/update-financas-request";
import { DataService } from "./data.service";

@Injectable({
  providedIn: "root",
})
export class FinancasService {
  #dataService = inject(DataService);

    getFinancas(id: number) {
        return this.#dataService.getFinancas(id);
    }

    searchFinancas(request: SearchFinancasRequest) {
        return this.#dataService.searchFinancas(request);
    }

    createFinancas(request: CreateFinancasRequest) {
        return this.#dataService.createFinancas(request);
    }

    updateFinancas(id: number, request: UpdateFinancasRequest) {
        return this.#dataService.updateFinancas(id, request);
    }

    deleteFinancas(id: number) {
        return this.#dataService.deleteFinancas(id);
    }
}
