import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { lastValueFrom } from 'rxjs/internal/lastValueFrom';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private httpClient: HttpClient = inject(HttpClient);
  protected title = 'client';
  protected members = signal<any>([]);


  async ngOnInit() {
    this.members.set(await this.getMembers());
    // this.httpClient.get('https://localhost:5107/api/members').subscribe({
    //   next: (response) => {
    //     this.members.set(response);
    //     console.log(response);
    //   },
    //   error: (error) => console.log(error),
    //   complete: () => console.log('Request completed')
    // });
  }

  async getMembers() {
    try {
      return lastValueFrom(this.httpClient.get('https://localhost:5107/api/members'));
    } catch (error) {
      console.error('Error fetching members:', error);
      throw error;
    }
  }
}
