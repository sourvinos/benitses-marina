import { Injectable } from '@angular/core'
import *  as CryptoJS from 'crypto-js'

@Injectable({ providedIn: 'root' })

export class CryptoService {

    private key = 'c56a334b-a1ad-4bad-b0bc-e9988721070a'

    public encrypt(txt: string): string {
        return CryptoJS.AES.encrypt(txt.toString(), this.key).toString()
    }

    public decrypt(txtToDecrypt: string): string {
        return CryptoJS.AES.decrypt(txtToDecrypt, this.key).toString(CryptoJS.enc.Utf8)
    }

}
