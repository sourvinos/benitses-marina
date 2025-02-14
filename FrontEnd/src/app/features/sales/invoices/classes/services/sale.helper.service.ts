import { Injectable } from '@angular/core'
// Custom
import { DateHelperService } from 'src/app/shared/services/date-helper.service'
import { DexieService } from 'src/app/shared/services/dexie.service'
import { DocumentTypeReadDto } from '../../../documentTypes/classes/dtos/documentType-read-dto'
import { ItemWriteDto } from '../dtos/form/item-write-dto'
import { SaleWriteDto } from '../dtos/form/sale-write-dto'

@Injectable({ providedIn: 'root' })

export class SaleHelperService {

    constructor(private dexieService: DexieService, private dateHelperService: DateHelperService) { }

    //#region public methods

    public flattenForm(form: any): SaleWriteDto {
        const x: SaleWriteDto = {
            invoiceId: form.invoiceId != '' ? form.invoiceId : null,
            date: this.dateHelperService.formatDateToIso(new Date(form.date)),
            invoiceNo: form.invoiceNo,
            customerId: form.customer.id,
            documentTypeId: form.documentType.id,
            paymentMethodId: form.paymentMethod.id,
            netAmount: form.saleSubTotal,
            vatAmount: form.saleVatAmount,
            grossAmount: form.saleGrossAmount,
            remarks: form.remarks,
            putAt: form.putAt,
            items: this.mapItems(form.items)
        }
        return x
    }

    public updateBrowserStorageAfterApiUpdate(record: DocumentTypeReadDto): void {
        this.dexieService.update('documentTypesSale', record)
    }

    //#endregion

    //#region private methods

    private mapItems(items: ItemWriteDto[]): ItemWriteDto[] {
        const z = []
        items.forEach((item: any) => {
            const x: ItemWriteDto = {
                invoiceId: item.invoiceId,
                code: item.code,
                description: item.description,
                englishDescription: item.englishDescription,
                qty: item.qty,
                netAmount: item.subTotal,
                vatPercent: item.vatPercent,
                vatAmount: item.vatAmount,
                grossAmount: item.grossAmount
            }
            z.push(x)
        })
        return z
    }

    //#endregion

}
