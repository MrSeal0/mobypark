import { readdir, readFile } from 'fs/promises';
import * as brulang from '@usebruno/lang'
const Excel = require('exceljs');

const ConvertToJson = (brunoFile) => {
	return brulang.bruToJsonV2(brunoFile)
}

const ConvertFilesToJson = async (path) => {
	let allFilesParsed = [];
	const files = await readdir(path);
	for (const filename of files) {
		if (!filename.endsWith('bru')) continue;
		const contents = (await readFile(path + '/' + filename)).toString();
		allFilesParsed.push(ConvertToJson(contents))
	}
	return allFilesParsed;

}


export const ConvertDataToPdf = async (data) => {
	const workbook = new Excel.Workbook();
	const worksheet = workbook.addWorksheet("Requests");

	worksheet.columns = [{ header: 'Method', key: 'method', width: 10 },
	{ header: 'Name', key: 'name', width: 20 },
	{ header: 'Url', key: 'url', width: 50, },
	{ header: 'Body-Type', key: 'bodytype', width: 10, },
	{ header: 'Body', key: 'body', width: 50, }
	];

	for (let i = 0; i < data.length; i++) {
		let req = data[i];
		worksheet.addRow({
			method: req?.http?.method ?? '',
			name: req?.meta?.name ?? '',
			url: req?.http?.url ?? '',
			bodytype: req?.http?.body ?? '',
			body: req?.body?.[req?.http?.body ?? ''] ?? ''
		});
	}

	await workbook.xlsx.writeFile('requests.xlsx');

	console.log("Created export of requests under 'requests.xlsx'");
};

ConvertFilesToJson('./moby-dick').then(data => ConvertDataToPdf(data))
