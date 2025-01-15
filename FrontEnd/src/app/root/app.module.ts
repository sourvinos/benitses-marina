// Base
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule, Title } from '@angular/platform-browser'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
import { NgModule } from '@angular/core'
import { registerLocaleData } from '@angular/common'
// Modules
import { AppRoutingModule } from './app.routing.module'
import { LoginModule } from '../shared/components/login/classes/modules/login.module'
import { SharedModule } from 'src/app/shared/modules/shared.module'
// Components
import { AppComponent } from './app.component'
import { CardsMenuComponent } from '../shared/components/home/cards-menu.component'
import { ExpensesMenuComponent } from '../shared/components/expenses-menu/expenses-menu.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LogoutComponent } from '../shared/components/logout/logout.component'
import { ReservationsMenuComponent } from '../shared/components/reservation-menu/reservations-menu.component'
import { UserMenuComponent } from '../shared/components/user-menu/user-menu.component'
// Services
import { InterceptorService } from '../shared/services/interceptor.service'
// Language
import localeEl from '@angular/common/locales/el'
import localeElExtra from '@angular/common/locales/extra/el'

registerLocaleData(localeEl, 'el', localeElExtra);

@NgModule({
    declarations: [
        AppComponent,
        CardsMenuComponent,
        ExpensesMenuComponent,
        HomeComponent,
        LogoutComponent,
        ReservationsMenuComponent,
        UserMenuComponent
    ],
    imports: [
        AppRoutingModule,
        BrowserAnimationsModule,
        BrowserModule,
        FormsModule,
        HttpClientModule,
        LoginModule,
        ReactiveFormsModule,
        SharedModule,
    ],
    providers: [
        Title, { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
