import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class Recipes {
  private apiUrl = `https://forkify-api.herokuapp.com/api/v2/recipes?search=pizza`;

  constructor(private _http: HttpClient) { }

  getRecipes(): Observable<any> {
    return this._http.get(this.apiUrl);
  }
}
