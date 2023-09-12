import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { enableProdMode } from '@angular/core';
import { environment } from 'environments/environment';

if (environment.production) {
  console.log("In production ---------------------------------")
  enableProdMode();
}else{
  
  console.log("In dev ---------------------------------")
}

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
