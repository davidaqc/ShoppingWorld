import { Injectable } from '@angular/core';
import {
  HttpInterceptor,
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Router, NavigationStart, NavigationEnd, NavigationCancel, NavigationError } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  private isNavigating = false;

  constructor(private router: Router, private snackBar: MatSnackBar) {
    // Subscribe to router events to track navigation
    this.router.events.subscribe(event => {
      if (event instanceof NavigationStart) {
        this.isNavigating = true;
      } else if (
        event instanceof NavigationEnd ||
        event instanceof NavigationCancel ||
        event instanceof NavigationError
      ) {
        this.isNavigating = false;
      }
    });
  }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        let errorMessage = '';

        if (error.error instanceof ErrorEvent) {
          errorMessage = 'Error: ' + error.error.message;
        } else {
          switch (error.status) {
            case 0:
              errorMessage = 'Error: Unable to connect to the server. Please check your internet connection.';
              break;
            case 400:
              errorMessage = error.error.message;
              break;
            case 401:
              errorMessage = error.error.message;
              break;
            case 403:
              errorMessage = error.error.message;
              break;
            case 404:
              errorMessage = error.error.message;
              this.router.navigate(['/not-found']);
              break;
            case 500:
              errorMessage = error.error.message;
              this.router.navigate(['/internal-server-error']);
              break;
            default:
              errorMessage = `Unexpected error: ${error.message}`;
              break;
          }
        }

        // Show the snackbar only if not navigating
        if (!this.isNavigating) {
          this.snackBar.open(errorMessage, 'Close', {
            duration: 5000,
            horizontalPosition: 'center',
            verticalPosition: 'top'
          });
        }

        // Return an observable with a user-facing error message
        return throwError({ message: errorMessage, status: error.status });
      })
    );
  }
}
