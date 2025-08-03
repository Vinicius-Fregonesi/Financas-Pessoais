import { Injectable, inject } from "@angular/core";
import { MessageService } from "primeng/api";

@Injectable({
  providedIn: "root",
})
export class ToastService {
  #messageService = inject(MessageService);

  show(message: string, title: string, severity: string, key: string) {
    this.#messageService.add({
      key: key,
      detail: message,
      severity: severity,
      summary: title,
    });
  }

  success(message: string, title: string = "Success") {
    this.show(message, title, "success", "global");
  }

  info(message: string, title: string = "Info") {
    this.show(message, title, "info", "global");
  }

  error(message: string, title: string = "Erro") {
    this.show(message, title, "error", "global");
  }

  errorMessages(messages: string[], title: string = "Erro") {
    const userFriendlyMessages = messages.filter(
      msg => !msg.includes(" at ") && !msg.includes("\\") && !msg.includes(".cs:")
    );
    const finalMessages = userFriendlyMessages.length > 0
      ? userFriendlyMessages
      : ["Erro inesperado."];

    finalMessages.forEach(msg => {
      this.error(msg, title);
    });
  }




  warn(message: string, title: string = "Warning") {
    this.show(message, title, "warn", "global");
  }
}
