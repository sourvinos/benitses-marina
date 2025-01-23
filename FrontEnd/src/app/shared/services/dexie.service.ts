import Dexie from 'dexie'
import { Injectable } from '@angular/core'
import { BerthHttpService } from 'src/app/features/reservations/berths/classes/services/berth-http.service'

@Injectable({ providedIn: 'root' })

export class DexieService extends Dexie {

    constructor() {
        super('BenitsesMarinaDB')
        this.version(1).stores({
            balanceFilters: 'id, description',
            balanceFiltersCriteria: 'id, description',
            banks: 'id, description',
            berths: 'id, description',
            boatTypes: 'id, description',
            boatUsages: 'id, description',
            companies: 'id, description',
            companiesCriteria: 'id, description',
            customers: 'id, description',
            expenseDocumentTypes: 'id, description',
            saleDocumentTypes: 'id, description',
            nationalities: 'id, description',
            paymentMethods: 'id, description',
            paymentStatuses: 'id, description',
            suppliers: 'id, description',
            suppliersCriteria: 'id, description',
            taxOffices: 'id, description'
        })
        this.open()
    }

    public populateTable(table: string, httpService: any): void {
        httpService.getForBrowser().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public populateCriteria(table: string, httpService: any): void {
        httpService.getForCriteria().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public populateNewTable(table: string, berthHttpService: BerthHttpService): void {
        berthHttpService.getForBrowser().subscribe((records: any) => {
            this.table(table)
                .clear().then(() => {
                    this.table(table)
                        .bulkAdd(records)
                        .catch(Dexie.BulkError, () => { })
                })
        })
    }

    public async getById(table: string, id: number): Promise<any> {
        return await this.table(table).get({ id: id })
    }

    public async getByDescription(table: string, description: string): Promise<any> {
        return await this.table(table).get({ description: description })
    }

    public async getByDefault(table: string, field: string): Promise<any> {
        return this.table(table).filter(x => x[field]).first()
    }

    public async getDefaultDocumentType(table: string, shipId: number): Promise<any> {
        return this.table(table).filter(x => x.ship.id == shipId && x.isDefault).first()
    }

    public update(table: string, item: any): void {
        this.table(table).put(item)
    }

    public remove(table: string, id: any): void {
        this.table(table).delete(id)
    }

}

export const db = new DexieService()
