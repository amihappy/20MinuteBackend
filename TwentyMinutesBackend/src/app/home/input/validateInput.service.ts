import { Injectable } from '@angular/core';

@Injectable()
export class ValidateInputService{

  public ValidateJson(input: string): boolean{
    try {
      JSON.parse(input);
    }
    catch {
      return false;
    }

    return true;
  }
}
