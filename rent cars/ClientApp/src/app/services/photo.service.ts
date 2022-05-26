import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class photoService {
  constructor(private http: HttpClient) {}

  upload(vehicleId, photo) {
    var formData = new FormData();
    formData.append('file', photo);
    return this.http.post(
      'https://localhost:7275/api/vehicles/' + vehicleId + '/photos',
      formData
    );
  }

  getPhotos(vehicleId) {
    return this.http.get<any>(
      'https://localhost:7275/api/vehicles/' + vehicleId + '/photos'
    );
  }
}
