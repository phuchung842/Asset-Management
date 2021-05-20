import React from "react";
import { useHistory, useParams } from "react-router";
import http from "../../ultis/httpClient";
import AssetForm from "./AssetForm";
import { stateOptions } from "../../enums/assetState";
import { useNSModals } from "../../containers/ModalContainer";

let params = {
  locationId: "9fdbb02a-244d-49ae-b979-362b4696479c",
};

const stateAssetEdit = stateOptions.slice(1);
const stateAssetCreate = stateOptions.filter(
  (item) => item.value === 2 || item.value === 3
);

export default function AssetDetail() {
  const { id } = useParams();
  const [dataEdit, setEdit] = React.useState(null);
  const [nameHeader, setnameHeader] = React.useState("");
  const [stateForm, setStateForm] = React.useState([]);
  const history = useHistory();
  //modal
  const { modalConfirm, modalLoading } = useNSModals();
  modalConfirm.config({
    message: "Save changed infomation of asset",
    btnName: "Ok",
    onSubmit: (asset) => {
      modalLoading.show();
      asset.locationId = params.locationId;
      if (id) {
        asset.assetId = id;
        http
          .put("/api/asset/" + id, asset)
          .then((resp) => {
            history.push("/assets");
          })
          .catch((err) => console.log(err))
          .finally(() => {
            modalLoading.close();
          });
      } else {
        http
          .post("/api/asset", asset)
          .then((resp) => {
            history.push("/assets");
          })
          .catch((err) => console.log(err))
          .finally(() => {
            modalLoading.close();
          });
      }
    },
  });

  React.useEffect(() => {
    if (id) {
      _fetchAssetData(id);
      setnameHeader("Edit Asset");
      setStateForm(stateAssetEdit);
    } else {
      setStateForm(stateAssetCreate);
      setnameHeader("Create New Asset");
    }
  }, [id]);

  const _fetchAssetData = (assetId) => {
    http
      .get("/api/asset/" + assetId)
      .then((resp) => {
        setEdit(resp.data);
      })
      .catch((err) => console.log(err));
  };

  const handleSubmit = (asset) => {
    modalConfirm.show(asset);
  };

  return (
    <>
      <h5 className="name-list">{nameHeader}</h5>
      <AssetForm
        data={dataEdit}
        listState={stateForm}
        onSubmit={handleSubmit}
      />
    </>
  );
}
