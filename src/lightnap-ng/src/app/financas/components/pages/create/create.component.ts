
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { BlockUiService, ErrorListComponent } from "@core";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { CheckboxModule } from "primeng/checkbox";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { finalize } from "rxjs";
import { CreateFinancasRequest, FinancaTipo } from "src/app/financas/models/request/create-financas-request";
import { FinancasService } from "src/app/financas/services/financas.service";

@Component({
  standalone: true,
  templateUrl: "./create.component.html",
  imports: [
    CommonModule,
    CardModule,
    ReactiveFormsModule,
    RouterLink,
    CalendarModule,
    ButtonModule,
    InputTextModule,
    InputNumberModule,
    CheckboxModule,
    ErrorListComponent,
    FormsModule
  ],
})
export class CreateComponent {
  #financasService = inject(FinancasService);
  #router = inject(Router);
  #activeRoute = inject(ActivatedRoute);
  #fb = inject(FormBuilder);
  #blockUi = inject(BlockUiService);

  errors = new Array<string>();
  tiposList = Object.keys(FinancaTipo)
  .filter(key => isNaN(Number(key)))  // filtra só as chaves texto (Despesa, Receita)
  .map(key => ({ name: key, value: FinancaTipo[key as keyof typeof FinancaTipo] }));


  form = this.#fb.group({
	// TODO: Update these fields to match the right parameters.
	descricao: this.#fb.control("Descricao", [Validators.required]),
	valor: this.#fb.control(0, [Validators.required]),
	data: this.#fb.control(new Date(), [Validators.required]),
  tipo: this.#fb.control(FinancaTipo.Despesa, [Validators.required]),
	categoria: this.#fb.control("Categoria", [Validators.required]),
  });

  createClicked() {
    this.errors = [];

    const request = <CreateFinancasRequest>this.form.value;

    this.#blockUi.show({message: "Creating..."});
    this.#financasService
      .createFinancas(request)
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: financas => this.#router.navigate([financas.id], { relativeTo: this.#activeRoute.parent }),
        error: response => (this.errors = response.errorMessages),
      });
  }
}