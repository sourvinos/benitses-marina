import Dexie from 'dexie'
import { Injectable } from '@angular/core'

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
            hullTypes: 'id, description',
            nationalities: 'id, description',
            paymentMethods: 'id, description, descriptionEn',
            paymentStatuses: 'id, description',
            periodTypes: 'id, description',
            prices: 'id, code, description',
            safes: 'id, description',
            safesCriteria: 'id, description',
            saleDocumentTypes: 'id, abbreviationEn',
            seasonTypes: 'id, description',
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

    public async getById(table: string, id: number): Promise<any> {
        return await this.table(table).get({ id: id })
    }

    public async getByDescription(table: string, description: string): Promise<any> {
        return await this.table(table).get({ description: description })
    }

    public update(table: string, item: any): void {
        this.table(table).put(item)
    }

    public remove(table: string, id: any): void {
        this.table(table).delete(id)
    }

}

export const db = new DexieService()
