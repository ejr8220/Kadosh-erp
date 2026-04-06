export type GridColumnType = 'text' | 'date' | 'boolean' | 'number';

export interface GridColumn {
  field: string;
  headerText: string;
  width?: number;
  textAlign?: 'Left' | 'Right' | 'Center';
  type?: GridColumnType;
  hidden?: boolean;
}
