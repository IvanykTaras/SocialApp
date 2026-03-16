import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners } from '@angular/core';
import { provideRouter, withViewTransitions } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient } from '@angular/common/http';
import { InitService } from '../core/services/init-service';
import { lastValueFrom } from 'rxjs';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideRouter(routes, withViewTransitions()),
    provideHttpClient(),
    provideAppInitializer(() => {
      const initService = inject(InitService);
      try {
        return lastValueFrom(initService.init());
      } finally {
        setTimeout(() => {
          const splash = document.getElementById('initial-splash');
          if(splash) splash.remove();
        }, 500);
      }
    }),
  ]
};
