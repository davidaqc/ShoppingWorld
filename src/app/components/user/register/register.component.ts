import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { UserService } from 'src/app/services/user/user.service';
import { Router } from '@angular/router';
import { Usuario } from 'src/app/interfaces/user/usuario';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  registroForm: FormGroup;
  mostrarContrasena: boolean = false;
  mostrarConfirmeContra: boolean = false;
  formSubmitted: boolean = false;
  showSuccess: boolean = false;

  constructor(private formBuilder: FormBuilder, private router: Router, private api: UserService, private snackBar: MatSnackBar) { 
    this.registroForm = this.formBuilder.group({
      nombre: ['', [Validators.required, Validators.maxLength(50)]],
      apellidos: ['', [Validators.required, Validators.maxLength(50)]],
      numeroIdentificacion: [null, [Validators.required, Validators.pattern(/^\d+$/)]],
      email: ['', [Validators.required, Validators.email]],
      contra: ['', [Validators.required, Validators.minLength(8), Validators.pattern(/[!@#$%^&*(),.?":{}|<>]/)]],
      confirmeContra: ['', Validators.required]
    }, {
      validators: this.passwordMatchValidator
    });
  }

  showSuccessMessage() {
    this.showSuccess = true;
    setTimeout(() => {
      this.showSuccess = false;
      this.router.navigate(["/productos"]);
    }, 3000);
  }

  ngOnInit(): void {
  }

  disableCopyPaste(event: ClipboardEvent) {
    event.preventDefault();
  }
  

  registrar() {
    this.formSubmitted = true;
    if (this.registroForm.valid) {
      const user = this.registroForm.value;

      const usuarioFinal: Usuario = {
        correo: user.email,
        nombre: user.nombre,
        apellidos: user.apellidos,
        user_password: user.contra,
        numero_identificacion: user.numeroIdentificacion
      };

      this.api.registrarUsuario(usuarioFinal)
        .subscribe(response => {
          this.showSuccessMessage();
        });
    }
  }

  togglePasswordVisibility() {
    this.mostrarContrasena = !this.mostrarContrasena;
  }

  toggleConfirmePasswordVisibility() {
    this.mostrarConfirmeContra = !this.mostrarConfirmeContra;
  }

  passwordMatchValidator(formGroup: FormGroup) {
    const password = formGroup.get('contra')?.value;
    const confirmPassword = formGroup.get('confirmeContra')?.value;

    if (password !== confirmPassword) {
      formGroup.get('confirmeContra')?.setErrors({ mismatch: true });
      return { mismatch: true };
    } else {
      return null;
    }
  }
}
