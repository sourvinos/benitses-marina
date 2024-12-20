// Base
import { NgModule } from '@angular/core'
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'
import { BrowserModule, Title } from '@angular/platform-browser'
import { FormsModule, ReactiveFormsModule } from '@angular/forms'
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http'
// Modules
import { AppComponent } from './app.component'
import { AppRoutingModule } from './app.routing.module'
import { LoginModule } from '../shared/components/login/classes/modules/login.module'
import { PrimeNgModule } from '../shared/modules/primeng.module'
import { SharedModule } from 'src/app/shared/modules/shared.module'
// Components
import { CardsMenuComponent } from '../shared/components/home/cards-menu.component'
import { ExpensesMenuComponent } from '../shared/components/expenses-menu/expenses-menu.component'
import { HomeComponent } from '../shared/components/home/home.component'
import { LogoutComponent } from '../shared/components/logout/logout.component'
import { ReservationsMenuComponent } from '../shared/components/reservation-menu/reservations-menu.component'
import { UserMenuComponent } from '../shared/components/user-menu/user-menu.component'
// Services
import { InterceptorService } from '../shared/services/interceptor.service'

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
        PrimeNgModule,
        ReactiveFormsModule,
        SharedModule,
    ],
    providers: [
        Title, { provide: HTTP_INTERCEPTORS, useClass: InterceptorService, multi: true }
    ],
    bootstrap: [AppComponent]
})

export class AppModule { }
