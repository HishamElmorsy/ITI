import { Pipe, PipeTransform } from '@angular/core';
import { Student } from '../interfaces/student';

@Pipe({
  name: 'search'
})
export class SearchPipe implements PipeTransform {

  transform(students: Student[], searchText: string): Student[] {
    if (!students || !searchText) {
      return students;
    }
    searchText = searchText.toLowerCase();
    return students.filter(s => s.firstName.toLowerCase().startsWith(searchText));
  }

}
