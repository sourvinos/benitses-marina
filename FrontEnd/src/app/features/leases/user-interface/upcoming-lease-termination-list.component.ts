import { ActivatedRoute, Router } from '@angular/router'
import { Component, ViewChild } from '@angular/core'
import { Table } from 'primeng/table'
// Custom
import { CryptoService } from 'src/app/shared/services/crypto.service'
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DialogService } from 'src/app/shared/services/modal-dialog.service'
import { FormBuilder, FormControl, FormGroup } from '@angular/forms'
import { HelperService } from 'src/app/shared/services/helper.service'
import { MessageDialogService } from 'src/app/shared/services/message-dialog.service'
import { MessageLabelService } from 'src/app/shared/services/message-label.service'
import { SessionStorageService } from 'src/app/shared/services/session-storage.service'
import { UpcomingLeaseTerminationListVM } from '../classes/view-models/upcoming-lease-termination-list-vm'

@Component({
    selector: 'upcoming-lease-termination-list',
    templateUrl: './upcoming-lease-termination-list.component.html',
    styleUrls: ['../../../../assets/styles/custom/lists.css', 'upcoming-lease-termination-list.component.css']
})

export class UpcomingLeaseTerminationListComponent {

    //#region variables

    @ViewChild('table') table: Table

    private url = 'upcomingLeaseTerminations'
    private virtualElement: any
    public form: FormGroup
    public feature = 'upcomingLeasesList'
    public featureIcon = 'leases'
    public icon = 'home'
    public parentUrl = '/home'
    public records: UpcomingLeaseTerminationListVM[] = []
    public recordsFilteredCount = 0

    public selectedRecords: UpcomingLeaseTerminationListVM[] = []

    public leaseDays: any[] = [
        { label: '30', value: '30' },
        { label: '45', value: '45' },
        { label: '60', value: '60' },
        { label: '75', value: '75' },
        { label: '90', value: '90' }
    ]

    //#endregion

    constructor(private activatedRoute: ActivatedRoute, private cryptoService: CryptoService, private dateHelperService: DateHelperService, private dialogService: DialogService, private formBuilder: FormBuilder, private helperService: HelperService, private messageDialogService: MessageDialogService, private messageLabelService: MessageLabelService, private router: Router, private sessionStorageService: SessionStorageService) { }

    //#region lifecycle hooks

    ngOnInit(): void {
        this.initForm()
        this.updateVariablesFromStorage()
        this.loadRecords()
        this.setTabTitle()
        this.setSidebarsHeight()
    }

    ngAfterViewInit(): void {
        setTimeout(() => {
            this.getVirtualElement()
            this.scrollToSavedPosition()
            this.enableDisableFilters()
        }, 500)
    }

    //#endregion

    //#region public methods

    public filterBySelection(): void {
        this.storeLeaseDays().then(() => {
            const currentUrl = this.router.url;
            this.router.navigate([currentUrl]);
        })
    }

    public formatDateToLocale(date: string, showWeekday = false, showYear = false, returnEmptyString = false): string {
        return returnEmptyString && date == '2199-12-31' ? '' : this.dateHelperService.formatISODateToLocale(date, showWeekday, showYear)
    }

    public getLabel(id: string): string {
        return this.messageLabelService.getDescription(this.feature, id)
    }

    public getOverdueDescription(isOverdue: boolean): string {
        return isOverdue ? 'YES' : ''
    }

    public isAdmin(): boolean {
        return this.cryptoService.decrypt(this.sessionStorageService.getItem('isAdmin')) == 'true' ? true : false
    }

    public onFilter(event: any, column: string, matchMode: string): void {
        if (event) this.table.filter(event, column, matchMode)
    }

    public onFilterRecords(event: any): void {
        this.sessionStorageService.saveItem(this.feature + '-' + 'filters', JSON.stringify(this.table.filters))
        this.recordsFilteredCount = event.filteredValue.length
    }

    public onHighlightRow(id: any): void {
        this.helperService.highlightRow(id)
    }

    public onNewRecord(): void {
        this.router.navigate([this.url + '/new'])
    }

    public onRefreshList(): void {
        // this.storeLeaseDays().then(() => {
        // const currentUrl = this.router.url;
        // this.router.navigate([currentUrl]);
        // })
    }

    public onResetTableFilters(): void {
        this.helperService.clearTableTextFilters(this.table, ['description', 'email', 'phones'])
    }

    //#endregion

    //#region private methods

    private enableDisableFilters(): void {
        this.records.length == 0 ? this.helperService.disableTableFilters() : this.helperService.enableTableFilters()
    }

    private getVirtualElement(): void {
        this.virtualElement = document.getElementsByClassName('p-scroller-inline')[0]
    }

    private goBack(): void {
        this.router.navigate([this.parentUrl])
    }

    private initForm(): void {
        this.form = this.formBuilder.group({
            value: new FormControl(),
        })
    }

    private loadRecords(): Promise<any> {
        return new Promise((resolve) => {
            const listResolved: any = this.activatedRoute.snapshot.data[this.feature]
            if (listResolved.error == null) {
                this.records = listResolved.list
                this.recordsFilteredCount = this.records.length
                resolve(this.records)
            } else {
                this.dialogService.open(this.messageDialogService.filterResponse(listResolved.error), 'error', ['ok']).subscribe(() => {
                    this.goBack()
                })
            }
        })
    }

    private scrollToSavedPosition(): void {
        this.helperService.scrollToSavedPosition(this.virtualElement, this.feature)
    }

    private setSidebarsHeight(): void {
        this.helperService.setSidebarsTopMargin('0')
    }

    private setTabTitle(): void {
        this.helperService.setTabTitle(this.feature)
    }

    private storeLeaseDays(): Promise<any> {
        return new Promise((resolve) => {
            this.sessionStorageService.saveItem('lease-days', this.form.value.value.toString())
            resolve(this.form.value.value)
        })
    }

    private updateVariablesFromStorage(): void {
        this.form.patchValue({
            value: this.sessionStorageService.getItem('lease-days').toString()
        })
    }

    //#endregion

}
