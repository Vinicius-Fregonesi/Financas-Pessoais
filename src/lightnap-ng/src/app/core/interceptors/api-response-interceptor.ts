import {
  HttpErrorResponse,
  HttpEvent,
  HttpHandlerFn,
  HttpRequest,
  HttpResponse,
} from "@angular/common/http";
import { inject } from "@angular/core";
import { ApiResponse, HttpErrorApiResponse } from "@core";
import { RouteAliasService } from "@routing";
import { Observable, catchError, of, switchMap, throwError } from "rxjs";
import { IdentityService } from "src/app/identity/services/identity.service";
import { environment } from "src/environments/environment";
import { ToastService } from "src/app/core/services/toast.service";

/**
 * Interceptor para tratar respostas da API e exibir mensagens amigáveis.
 */
export function apiResponseInterceptor(
  request: HttpRequest<unknown>,
  next: HttpHandlerFn
): Observable<HttpEvent<unknown>> {
  const identityService = inject(IdentityService);
  const routeAliasService = inject(RouteAliasService);
  const toastService = inject(ToastService);

  return next(request).pipe(
    switchMap((httpEvent) => {
      if (
        httpEvent instanceof HttpResponse &&
        isApiResponse(httpEvent.body)
      ) {
        const { type, errorMessages, result } = httpEvent.body;

        if (["Error", "UnexpectedError"].includes(type)) {
          const mensagens = filterUserFriendlyMessages(errorMessages ?? []);
          toastService.errorMessages(mensagens);
          return throwError(() => httpEvent.body);
        }

        if (type === "Success") {
          return of(httpEvent.clone({ body: result ?? null }));
        }
      }

      return of(httpEvent);
    }),

    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        identityService.logOut();
        routeAliasService.navigate("login");
        return throwError(() => error);
      }

      const errorBody = error.error;
      const mensagens =
        isApiResponse(errorBody) && errorBody.errorMessages
          ? errorBody.errorMessages
          : typeof errorBody === "string"
          ? [errorBody]
          : ["Erro inesperado."];

      toastService.errorMessages(filterUserFriendlyMessages(mensagens));

      if (!environment.production) {
        console.warn("Erro técnico:", error);
      }

      return throwError(() => new HttpErrorApiResponse(error));
    })
  );
}

/**
 * Verifica se o corpo da resposta segue o formato ApiResponse.
 */
function isApiResponse(body: any): body is ApiResponse<unknown> {
  return body && typeof body === "object" && "type" in body;
}

/**
 * Remove mensagens técnicas como stack traces e caminhos de arquivos.
 */
function filterUserFriendlyMessages(messages: string[]): string[] {
  return messages.filter(
    (msg) =>
      !msg.includes(" at ") &&
      !msg.includes("\\") &&
      !msg.includes(".cs:")
  );
}
