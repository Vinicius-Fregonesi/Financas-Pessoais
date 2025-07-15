
import { CommonModule } from "@angular/common";
import { Component, effect, inject, input } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponseComponent } from "@core";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { Observable } from "rxjs";
import { FinancaTipo } from "src/app/financas/models/request/create-financas-request";
import { Financas } from "src/app/financas/models/response/financas";
import { FinancasService } from "src/app/financas/services/financas.service";

@Component({
  standalone: true,
  templateUrl: "./get.component.html",
  imports: [CommonModule, CardModule, RouterLink, ApiResponseComponent, ButtonModule],
})
export class GetComponent {
  #financasService = inject(FinancasService);
  errors = new Array<string>();
  tipos= FinancaTipo;

  readonly id = input.required<number>();
  financas$ = new Observable<Financas>();

  constructor() {
    effect(() => this.financas$ = this.#financasService.getFinancas(this.id()));
  }
}
