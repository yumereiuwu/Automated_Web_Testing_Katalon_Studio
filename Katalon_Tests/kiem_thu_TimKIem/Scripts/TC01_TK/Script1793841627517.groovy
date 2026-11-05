import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.llm.keyword.LlmKeywords as LLM
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

WebUI.openBrowser(null)

WebUI.navigateToUrl('http://localhost:61445/')

WebUI.click(findTestObject('TEST/Page_Trang ch  n vt ngon/a_NG NHP'))

WebUI.setText(findTestObject('TEST/Page_ng nhp  n vt ngon/input_email'), 'an@')

WebUI.setEncryptedText(findTestObject('TEST/Page_ng nhp  n vt ngon/input_loginPassword'), 'aeHFOx8jV/A=')

WebUI.sendKeys(findTestObject('TEST/Page_ng nhp  n vt ngon/input_loginPassword'), Keys.chord(Keys.ENTER))

WebUI.setText(findTestObject('TEST/Page_ng nhp  n vt ngon/input_email'), 'an@gmail.com')

WebUI.sendKeys(findTestObject('TEST/Page_ng nhp  n vt ngon/input_email'), Keys.chord(Keys.ENTER))

WebUI.setText(findTestObject('TEST/Page_Trang ch  n vt ngon/input_Tm kim'), 'bánh')

WebUI.click(findTestObject('TEST/Page_Trang ch  n vt ngon/strong_Bnh Bng Lan Hura 25g'))

WebUI.setText(findTestObject('Page_Bnh Bng Lan Hura 25g  n vt ngon/input_Tm kim'), '')

WebUI.click(findTestObject('TEST/Page_Bnh Bng Lan Hura 25g  n vt ngon/a_TRANG CH'))

WebUI.click(findTestObject('TEST/Page_Trang ch  n vt ngon/a_GII THIU'))

WebUI.click(findTestObject('TEST/Page_Gii thiu  n vt ngon/a_SN PHM'))

WebUI.click(findTestObject('TEST/Page_Gii thiu  n vt ngon/a_Tt c sn phm'))

WebUI.click(findTestObject('TEST/Page_Sn phm  n vt ngon/a_n healthy'))

WebUI.click(findTestObject('TEST/Page_Sn phm  n vt ngon/div_DANH MC SN PHM'))

WebUI.click(findTestObject('TEST/Page_Sn phm  n vt ngon/a_Bnh cm'))

WebUI.click(findTestObject('TEST/Page_Sn phm  n vt ngon/a_Tt c sn phm'))

WebUI.click(findTestObject('TEST/Page_Sn phm  n vt ngon/img_Bnh Bng Lan Hura 25g'))

WebUI.click(findTestObject('TEST/Page_Bnh Bng Lan Hura 25g  n vt ngon/button_Thm vo gi'))

WebUI.click(findTestObject('TEST/Page_Gi hng  n vt ngon/a_t hng  Thanh ton'))

WebUI.selectOptionByValue(findTestObject('TEST/Page_t hng  n vt ngon/select_HinhThucThanhToan'), 'MoMo', false)

WebUI.click(findTestObject('TEST/Page_t hng  n vt ngon/button_Xc nhn t hng'))

WebUI.click(findTestObject('TEST/Page_Ch thanh ton MoMo  n vt ngon/a_To li thanh ton MoMo'))

WebUI.click(findTestObject('TEST/Page_Ch thanh ton MoMo  n vt ngon/a_V gi hng'))

WebUI.click(findTestObject('TEST/Page_Gi hng  n vt ngon/a_t hng  Thanh ton'))

WebUI.selectOptionByValue(findTestObject('TEST/Page_t hng  n vt ngon/select_HinhThucThanhToan'), 'MoMo', false)

WebUI.selectOptionByValue(findTestObject('TEST/Page_t hng  n vt ngon/select_HinhThucThanhToan'), 'ChuyenKhoan', false)

WebUI.click(findTestObject('TEST/Page_t hng  n vt ngon/button_Xc nhn t hng'))

WebUI.selectOptionByValue(findTestObject('TEST/Page_t hng  n vt ngon/select_HinhThucThanhToan'), 'TienMat', false)

WebUI.click(findTestObject('TEST/Page_t hng  n vt ngon/button_Xc nhn t hng'))

WebUI.click(findTestObject('TEST/Page_t hng thnh cng  n vt ngon/a_LIN H'))

WebUI.click(findTestObject('TEST/Page_t hng thnh cng  n vt ngon/a_TIN TC'))

WebUI.click(findTestObject('Page_Tin tc  n vt ngon/a_LIN H'))

WebUI.click(findTestObject('TEST/Page_Lin h  n vt ngon/input_Nhp h tn'))

WebUI.click(findTestObject('TEST/Page_Lin h  n vt ngon/a_CHNH SCH'))

WebUI.click(findTestObject('TEST/Page_Lin h  n vt ngon/a_Giao hng'))

WebUI.click(findTestObject('Page_Chnh sch giao hng  n vt ngon/a_CHNH SCH'))

WebUI.click(findTestObject('TEST/Page_Chnh sch giao hng  n vt ngon/a_i tr'))

WebUI.click(findTestObject('Page_Chnh sch i tr  n vt ngon/a_CHNH SCH'))

WebUI.click(findTestObject('TEST/Page_Chnh sch i tr  n vt ngon/a_Bo mt'))

WebUI.click(findTestObject('TEST/Page_Chnh sch bo mt  n vt ngon/a_FAQ'))

WebUI.click(findTestObject('TEST/Page_Cu hi thng gp  n vt ngon/p_C. Shop h tr giao hng trn ton quc thng'))

WebUI.doubleClick(findTestObject('TEST/Page_Cu hi thng gp  n vt ngon/p_C. Shop h tr giao hng trn ton quc thng'))

WebUI.click(findTestObject('TEST/Page_Cu hi thng gp  n vt ngon/p_C. Shop h tr giao hng trn ton quc thng'))

WebUI.click(findTestObject('TEST/Page_Cu hi thng gp  n vt ngon/div_div'))

WebUI.click(findTestObject('Page_Cu hi thng gp  n vt ngon/a_TRANG CH'))

WebUI.click(findTestObject('TEST/Page_Trang ch  n vt ngon/a_NG XUT'))

WebUI.click(findTestObject('TEST/Page_Trang ch  n vt ngon/a_NG K'))

WebUI.setText(findTestObject('TEST/Page_ng k  n vt ngon/input_HoTen'), 'nguyen thuy dung')

WebUI.setText(findTestObject('TEST/Page_ng k  n vt ngon/input_Email'), 'manh123@gmail.com')

WebUI.setEncryptedText(findTestObject('TEST/Page_ng k  n vt ngon/input_MatKhau'), 'aeHFOx8jV/A=')

WebUI.setEncryptedText(findTestObject('TEST/Page_ng k  n vt ngon/input_MatKhauXacNhan'), 'aeHFOx8jV/A=')

WebUI.setText(findTestObject('TEST/Page_ng k  n vt ngon/input_SoDienThoai'), '0987206090')

WebUI.setText(findTestObject('TEST/Page_ng k  n vt ngon/input_DiaChi'), 'yên bái')

WebUI.click(findTestObject('TEST/Page_ng k  n vt ngon/button_ng k'))

