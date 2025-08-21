import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminUserOverview } from './admin-user-overview';

describe('AdminUserOverview', () => {
  let component: AdminUserOverview;
  let fixture: ComponentFixture<AdminUserOverview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdminUserOverview]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminUserOverview);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
