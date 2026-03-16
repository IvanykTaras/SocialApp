import { HttpInterceptorFn } from '@angular/common/http';
import { catchError } from 'rxjs';
import { ToastService } from '../services/toast-service';
import { inject } from '@angular/core/primitives/di';
import { Router } from '@angular/router';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastService = inject(ToastService);
  const router = inject(Router);
  return next(req).pipe(
    catchError((error) => {
      if(error){
        switch (error.status) {
          case 400:
            toastService.error(JSON.stringify(error.error));
            break;
          case 401:
            toastService.error(JSON.stringify(error.error));
            break;
          case 404:
            router.navigateByUrl('/not-found');
            toastService.error(JSON.stringify(error.error));
            break;
          case 500:
            toastService.error(JSON.stringify(error.error));
            break;
          default:
            toastService.error("Some error occurred");
        }
      }
      throw error;
    })
  );
};
