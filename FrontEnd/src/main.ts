import { enableProdMode } from '@angular/core'
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic'
import { AppModule } from '../src/app/root/app.module'
import { environment } from './environments/environment'

if (environment.isProduction) {
    enableProdMode()
}

platformBrowserDynamic().bootstrapModule(AppModule)
    .catch(err => console.error(err))
