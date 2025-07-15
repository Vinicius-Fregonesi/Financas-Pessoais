import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormBuilder, FormsModule, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ApiResponseComponent, EmptyPagedResponse } from "@core";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { PanelModule } from "primeng/panel";
import { TableLazyLoadEvent, TableModule } from "primeng/table";
import { CheckboxModule } from "primeng/checkbox";
import { debounceTime, startWith, Subject, switchMap, tap } from "rxjs";
import { Financas } from "src/app/financas/models/response/financas";
import { FinancasService } from "src/app/financas/services/financas.service";
import { ChartModule } from 'primeng/chart';
import { FinancaTipo } from "src/app/financas/models/request/create-financas-request";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    InputTextModule,
    InputNumberModule,
    CalendarModule,
    CheckboxModule,
    ApiResponseComponent,
    PanelModule,
    TableModule,
    RouterModule,
    ButtonModule,
    ChartModule,
    FormsModule
  ],
})
export class IndexComponent {
  pageSize = 10;
  tipoOptions = [
    FinancaTipo.Receita,
    FinancaTipo.Despesa
  ];
  getTipoDescricao(tipo: FinancaTipo): string {
    return tipo === FinancaTipo.Receita ? 'Receita' :
      tipo === FinancaTipo.Despesa ? 'Despesa' : '';
  }

  #financasService = inject(FinancasService);
  #fb = inject(FormBuilder);

  form = this.#fb.group({
    descricao: this.#fb.nonNullable.control<string | undefined>(undefined),
    valor: this.#fb.nonNullable.control<number | undefined>(undefined),
    datainicio: this.#fb.nonNullable.control<Date | undefined>(undefined), // ✅ ajustado
    datafinal: this.#fb.nonNullable.control<Date | undefined>(undefined),
    tipo: this.#fb.nonNullable.control<FinancaTipo | undefined>(undefined), // <- aqui
    categoria: this.#fb.nonNullable.control<string | undefined>(undefined),
  });


  total = 0;
  totalReceitas = 0;
  totalDespesas = 0;

  chartData: any;
  chartOptions: any = {
    plugins: {
      legend: {
        position: 'bottom',
      },
    },
  };

  loading = false; // controla o spinner da tabela

  #lazyLoadEventSubject = new Subject<TableLazyLoadEvent>();

  searchResults$ = this.#lazyLoadEventSubject.pipe(
    tap(() => {
      this.loading = true; // inicia loading quando uma nova requisição for disparada
    }),
    switchMap(event =>
      this.#financasService.searchFinancas({
        ...this.form.value,
        pageSize: this.pageSize,
        pageNumber: (event.first ?? 0) / this.pageSize + 1,
      }).pipe(
        tap(response => {
          const financas = response.data;

          this.totalReceitas = financas
            .filter(f => f.tipo === 'Receita')
            .reduce((acc, f) => acc + f.valor, 0);

          this.totalDespesas = financas
            .filter(f => f.tipo === 'Despesa')
            .reduce((acc, f) => acc + f.valor, 0);

          this.total = this.totalReceitas - this.totalDespesas;
          financas.forEach(f => console.log('tipo:', f.tipo, typeof f.tipo));

          const categorias = financas.reduce((acc, f) => {
            const categoria = f.categoria || 'Outros';
            const valor = f.tipo === FinancaTipo.Despesa ? -f.valor : f.valor;
            acc[categoria] = (acc[categoria] || 0) + valor;
            return acc;
          }, {} as Record<string, number>);




          this.chartData = {
            labels: Object.keys(categorias),
            datasets: [
              {
                data: Object.values(categorias),
                backgroundColor: ['#42A5F5', '#66BB6A', '#FFA726', '#EF5350', '#AB47BC'],
              },
            ],
          };

          this.loading = false; // finaliza loading ao receber os dados
        })
      )
    ),
    startWith(new EmptyPagedResponse<Financas>())
  );

  constructor() {
    this.form.valueChanges.pipe(takeUntilDestroyed(), debounceTime(1000)).subscribe(() => {
      this.#lazyLoadEventSubject.next({ first: 0 });
    });

    this.#lazyLoadEventSubject.next({ first: 0 });
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.#lazyLoadEventSubject.next(event);
  }
}
