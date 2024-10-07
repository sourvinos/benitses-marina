import { Component } from '@angular/core'
// Custom
import { LocalStorageService } from '../../services/local-storage.service'

@Component({
    selector: 'logo',
    templateUrl: './logo.component.html',
    styleUrls: ['./logo.component.css']
})

export class LogoComponent {

    constructor(private localStorageService: LocalStorageService) { }

    public getLogo(): string {
        return '../../../../assets/images/logos/' + 'logo-' + this.localStorageService.getItem('theme') + '.svg'
    }

}
