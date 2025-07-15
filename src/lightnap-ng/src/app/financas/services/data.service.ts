
import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, PagedResponse } from "@core";
import { tap } from "rxjs";
import {FinancasHelper } from "../helpers/financas.helper";
import { CreateFinancasRequest } from "../models/request/create-financas-request";
import { SearchFinancasRequest } from "../models/request/search-financas-request";
import { UpdateFinancasRequest } from "../models/request/update-financas-request";
import { Financas } from "../models/response/financas";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}Financas/`;

  getFinancas(id: number) {
    return this.#http.get<Financas>(`${this.#apiUrlRoot}${id}`).pipe(
      tap(financas => FinancasHelper.rehydrate(financas))
      );
  }

  searchFinancas(request: SearchFinancasRequest) {
    return this.#http.post<PagedResponse<Financas>>(`${this.#apiUrlRoot}search`, request).pipe(
      tap(results => results.data.forEach(FinancasHelper.rehydrate))
    );
  }

  createFinancas(request: CreateFinancasRequest) {
    return this.#http.post<Financas>(`${this.#apiUrlRoot}`, request);
  }

  updateFinancas(id: number, request: UpdateFinancasRequest) {
    return this.#http.put<Financas>(`${this.#apiUrlRoot}${id}`, request);
  }

  deleteFinancas(id: number) {
    return this.#http.delete<boolean>(`${this.#apiUrlRoot}${id}`);
  }
}
