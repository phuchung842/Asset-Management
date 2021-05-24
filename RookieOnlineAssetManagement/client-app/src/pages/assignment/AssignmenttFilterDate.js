import React from "react";
import { Input } from "reactstrap";
import { formatDate } from "../../ultis/helper";

export default function AssignmenttFilterState({ onChange }) {
  const handleChangeDate = (event) => {
    let date = formatDate(event.target.value);
    onChange && onChange(date);
  };

  return (
    <Input
      type="date"
      name="dateAddAsset"
      // value={dateCurrent}
      defaultValue=""
      onChange={handleChangeDate}
    />
  );
}