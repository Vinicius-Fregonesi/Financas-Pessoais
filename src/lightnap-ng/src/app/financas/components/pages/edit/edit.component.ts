
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnInit } from "@angular/core";
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { ApiResponseComponent, BlockUiService, ConfirmPopupComponent, ErrorListComponent, ToastService } from "@core";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { CheckboxModule } from "primeng/checkbox";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { finalize, Observable, tap } from "rxjs";
import { FinancaTipo } from "src/app/financas/models/request/create-financas-request";
import { UpdateFinancasRequest } from "src/app/financas/models/request/update-financas-request";
import { Financas } from "src/app/financas/models/response/financas";
import { FinancasService } from "src/app/financas/services/financas.service";

@Component({
  standalone: true,
  templateUrl: "./edit.component.html",
  imports: [
    CommonModule,
    CardModule,
    ReactiveFormsModule,
    ApiResponseComponent,
    ConfirmPopupComponent,
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
export class EditComponent implements OnInit {
  #financasService = inject(FinancasService);
  #router = inject(Router);
  #activeRoute = inject(ActivatedRoute);
  #confirmationService = inject(ConfirmationService);
  #toast = inject(ToastService);
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

  readonly id = input.required<number>();
  financas$ = new Observable<Financas>();

  ngOnInit() {
    this.financas$ = this.#financasService.getFinancas(this.id()).pipe(
      tap(financas => this.form.patchValue(financas))
    );
  }

  saveClicked() {
    this.errors = [];

    const request = <UpdateFinancasRequest>this.form.value;

    this.#blockUi.show({ message: "Saving..." });
    this.#financasService
      .updateFinancas(this.id(), request)
      .pipe(finalize(() => this.#blockUi.hide()))
      .subscribe({
        next: () => this.#toast.success("Updated successfully"),
        error: response => (this.errors = response.errorMessages),
      });
  }
  
  deleteClicked(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Delete Item",
      message: `Are you sure that you want to delete this item?`,
      target: event.target,
      key: "delete",
      accept: () => {
        this.#blockUi.show({ message: "Deleting..." });
        this.#financasService.deleteFinancas(this.id())
          .pipe(finalize(() => this.#blockUi.hide()))
          .subscribe({
            next: () => {
              this.#toast.success("Deleted successfully");
              this.#router.navigate(["."], { relativeTo: this.#activeRoute.parent });
            },
            error: response => this.errors = response.errorMessages
          });
      },
    });
  }
}