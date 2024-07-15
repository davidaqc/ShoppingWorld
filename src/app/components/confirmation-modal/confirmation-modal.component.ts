import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
    selector: 'app-confirmation-modal',
    templateUrl: './confirmation-modal.component.html',
    styleUrls: ['./confirmation-modal.component.css']
})
export class ConfirmationModalComponent {
    @Input() showConfirmation = false;
    @Output() confirmAction = new EventEmitter<void>();
    @Output() cancelAction = new EventEmitter<void>();

    cerrarConfirmacion() {
        this.cancelAction.emit();
    }

    confirmarEliminacion() {
        this.confirmAction.emit();
    }

    cancelarEliminacion() {
        this.cancelAction.emit();
    }
}
